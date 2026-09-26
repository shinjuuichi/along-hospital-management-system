using AutoMapper;
using BillingSvc.BLL.DTOs;
using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.BLL.Interfaces;
using BillingSvc.BLL.StateMachines;
using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalOrderEvents;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;

namespace BillingSvc.BLL.Implements
{
    public class RefundService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IMessageBus messageBus)
        : IRefundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        private readonly IGenericRepository<Invoice> _invoiceRepository = unitOfWork.Repository<Invoice>();
        private readonly IGenericRepository<Refund> _refundRepository = unitOfWork.Repository<Refund>();

        private const int REFUND_EXPIRATION_DAYS = 7;

        private readonly string[] invoiceIncludeCharges = [$"{nameof(Invoice.Charges)}.{nameof(Charge.Refund)}"];
        private readonly string[] refundIncludeInvoiceAndCharge = [$"{nameof(Refund.Charge)}.{nameof(Charge.Invoice)}"];

        public async Task<InvoiceAndChargeIdDTO> GetOrCreateInvoiceAndChargeIdByClinicalMedicalOrderDetailIdAsync(string clinicalMedicalOrderDetailId)
        {
            var refund = await _refundRepository.GetByConditionAsync(
                r => r.ClinicalMedicalOrderDetailId == clinicalMedicalOrderDetailId,
                refundIncludeInvoiceAndCharge);

            if (refund == null)
            {
                var createRefundContract = await _messageBus.RequestAsync<
                    GetRefundDataByClinicalMedicalOrderDetailIdEvent,
                    CreateRefundContract>(new() { ClinicalMedicalOrderDetailId = clinicalMedicalOrderDetailId });

                var clinicalMedicalOrderId = createRefundContract.ClinicalMedicalOrderId;
                if (string.IsNullOrEmpty(clinicalMedicalOrderId))
                {
                    throw new InvalidDataException("Clinical Medical Order Id is required");
                }

                var createChargeDTO = _mapper.Map<CreateChargeDTO>(createRefundContract);
                return await this.CreateByMedicalOrderDataAsync(clinicalMedicalOrderId, createChargeDTO);
            }

            if (refund.Charge == null)
            {
                throw new DataNotFoundException($"Charge for Refund with Clinical Medical Order Detail {clinicalMedicalOrderDetailId} not found.");
            }

            return new(refund.Charge.InvoiceId, refund.ChargeId);
        }

        public async Task<InvoiceAndChargeIdDTO> CreateByMedicalOrderDataAsync(string clinicalMedicalOrderId, CreateChargeDTO createChargeDTO)
        {
            var completedInvoice = await _invoiceRepository.GetByConditionAsync(i =>
                    i.ClinicalMedicalOrderId == clinicalMedicalOrderId
                    && i.InvoiceStatus == InvoiceStatusEnum.Completed, invoiceIncludeCharges)
                ?? throw new DataNotFoundException($"Clinical Medical Order ({clinicalMedicalOrderId}) not have a completed invoice.");

            var isRefundExist = await _refundRepository.AnyAsync(r =>
                    r.Charge != null
                    && r.Charge.MedicalServiceId == createChargeDTO.MedicalServiceId
                    && r.Charge.InvoiceId == completedInvoice.Id);
            if (isRefundExist)
            {
                throw new DataConflictException($"Refund for Medical Service ({createChargeDTO.MedicalServiceId}) in Clinical Medical Order ({clinicalMedicalOrderId}) already exists.");
            }

            var medicalServiceContract = await _messageBus.RequestAsync<
                GetMedicalServiceByIdEvent,
                GetMedicalServiceContract>(new() { Id = createChargeDTO.MedicalServiceId });

            createChargeDTO.UnitPrice = medicalServiceContract.Price;
            createChargeDTO.ChargeSnapshot = _mapper.Map<CreateChargeSnapshotDTO>(medicalServiceContract);

            var charge = _mapper.Map<Charge>(createChargeDTO);
            completedInvoice.Charges.Add(charge);

            _invoiceRepository.Update(completedInvoice);
            await _unitOfWork.SaveChangeAsync();

            return new(completedInvoice.Id, charge.Id);
        }

        public async Task UpdateStatusByChargeIdAsync(int chargeId, RefundStatusEnum refundStatus)
        {
            var refund = await _refundRepository.GetByConditionAsync(r => r.ChargeId == chargeId)
                ?? throw new DataNotFoundException($"Refund with ChargeId {chargeId} not found.");

            this.UpdateStatus(refund, refundStatus);
            _refundRepository.Update(refund);
            await _unitOfWork.SaveChangeAsync();
        }

        private void UpdateStatus(Refund refund, RefundStatusEnum refundStatus)
        {
            var stateMachine = new RefundStatusStateMachine(refund, _currentUserService.UserId);
            if (!stateMachine.CanFire(refundStatus))
            {
                throw new InvalidOperationException($"Cannot change refund status from {refund.RefundStatus} to {refundStatus}.");
            }
            try
            {
                stateMachine.Fire(refundStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to update refund status: {e.Message}");
            }
        }

        public async Task CancelExpiredRefundsAsync()
        {
            var dateTimeNow = DateTime.UtcNow;

            var expiredRefunds = await _refundRepository.GetAllAsync(r =>
                r.RefundStatus == RefundStatusEnum.Pending
                && r.CreationDate.AddDays(REFUND_EXPIRATION_DAYS) < dateTimeNow);

            List<Refund> successfullyCancelledRefunds = [];

            foreach (var refund in expiredRefunds)
            {
                try
                {
                    this.UpdateStatus(refund, RefundStatusEnum.Cancelled);
                    successfullyCancelledRefunds.Add(refund);
                }
                catch
                {
                    continue;
                }
            }

            if (successfullyCancelledRefunds.Any())
            {
                _refundRepository.UpdateRange(successfullyCancelledRefunds);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
