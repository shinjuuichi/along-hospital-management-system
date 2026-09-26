using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.BLL.StateMachines;
using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using MedicalOrderSvc.DAL.Models.Snapshots;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.MedicalOrderContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.BillingEvents;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicalOrderSvc.BLL.Implements
{
    public class ClinicalMedicalOrderService(
        IMongoGenericRepository<ClinicalMedicalOrder> clinicalMedicalOrderRepository,
        IMapper mapper,
        IMessageBus messageBus)
            : IClinicalMedicalOrderService
    {
        private readonly IMongoGenericRepository<ClinicalMedicalOrder> _clinicalMedicalOrderRepository = clinicalMedicalOrderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<(string MedicalOrderId, GetClinicalMedicalOrderDetailDTO ClinicalMedicalOrderDetail)> GetDetailByIdAsync(string clinicalMedicalOrderDetailId)
        {
            var clinicalMedicalOrder = await _clinicalMedicalOrderRepository.GetByConditionAsync(order =>
                order.ClinicalMedicalOrderDetails.Any(detail => detail.Id == clinicalMedicalOrderDetailId));
            if (clinicalMedicalOrder == null)
            {
                throw new DataNotFoundException(typeof(ClinicalMedicalOrderDetail), clinicalMedicalOrderDetailId);
            }

            var clinicalMedicalOrderDetail = clinicalMedicalOrder.ClinicalMedicalOrderDetails
                .First(detail => detail.Id == clinicalMedicalOrderDetailId);

            return (clinicalMedicalOrder.Id,
                _mapper.Map<GetClinicalMedicalOrderDetailDTO>(clinicalMedicalOrderDetail));
        }

        public async Task<GetMedicalOrderDTO> CreateAsync(CreateClinicalMedicalOrderDTO createDTO)
        {
            // Validate Medical Service Ids
            var medicalServiceIds = createDTO.ClinicalMedicalOrderDetails
                .Select(d => d.MedicalServiceId)
                .ToList();

            if (medicalServiceIds.Count == 0)
            {
                throw new InvalidDataException("At least one Medical Service Id is required in Clinical Medical Order Details.");
            }

            var hasDuplicatedMedicalService = medicalServiceIds.Count != medicalServiceIds.Distinct().Count();
            if (hasDuplicatedMedicalService)
            {
                throw new InvalidDataException("Duplicate Medical Service Ids are not allowed in Clinical Medical Order Details.");
            }

            // Request Medical Service data from Medical Service Service
            var medicalServiceContracts = await _messageBus.RequestAsync<GetListMedicalServiceDataByIdsEvent, GetListMedicalServiceDataContract>(new()
            {
                Ids = medicalServiceIds
            });

            var medicalServiceDict = medicalServiceContracts.Data.ToDictionary(ms => ms.Id);

            // Map to ClinicalMedicalOrder and set MedicalServiceSnapshot for each ClinicalMedicalOrderDetail
            var clinicalMedicalOrder = _mapper.Map<ClinicalMedicalOrder>(createDTO);
            foreach (var detail in clinicalMedicalOrder.ClinicalMedicalOrderDetails)
            {
                if (!medicalServiceDict.TryGetValue(detail.MedicalServiceId, out var medicalService))
                {
                    throw new DataNotFoundException("Medical Service", detail.MedicalServiceId);
                }

                detail.MedicalServiceSnapshot = _mapper.Map<MedicalServiceSnapshot>(medicalService);
            }

            // Create Clinical Medical Order and return DTO with pending invoice id
            var createdClinicalMedicalOrder = await _clinicalMedicalOrderRepository.AddAsync(clinicalMedicalOrder);

            var getClinicalMedicalOrderDTO = _mapper.Map<GetClinicalMedicalOrderDTO>(createdClinicalMedicalOrder);
            await this.RequestValueForDTOsAsync([getClinicalMedicalOrderDTO]);
            return getClinicalMedicalOrderDTO;
        }

        public async Task RequestValueForDTOsAsync(List<GetClinicalMedicalOrderDTO> clinicalMedicalOrderDTOs)
        {
            var pendingPaymentClinicalMedicalOrderDTOs = clinicalMedicalOrderDTOs
                .Where(dto => dto.Id != null
                        && dto.ClinicalMedicalOrderStatus == nameof(ClinicalMedicalOrderStatusEnum.Pending))
                .ToList();

            if (pendingPaymentClinicalMedicalOrderDTOs.Count == 0)
            {
                return;
            }

            var pendingInvoiceEvent = new GetListPendingInvoiceDataByClinicalMedicalOrderIdsEvent()
            {
                ClinicalMedicalOrderDict = pendingPaymentClinicalMedicalOrderDTOs.ToDictionary(
                        dto => dto.Id!,
                        _mapper.Map<GetClinicalMedicalOrderContract>)
            };
            var pendingInvoiceContracts = await _messageBus.RequestAsync<
                GetListPendingInvoiceDataByClinicalMedicalOrderIdsEvent,
                GetListInvoiceDataContract>(pendingInvoiceEvent);

            Dictionary<string, GetInvoiceContract> pendingInvoiceDict = pendingInvoiceContracts.Data
                .Where(invoice => !string.IsNullOrWhiteSpace(invoice.ClinicalMedicalOrderId))
                .ToDictionary(invoice => invoice.ClinicalMedicalOrderId!);

            foreach (var medicalOrderDTO in pendingPaymentClinicalMedicalOrderDTOs)
            {
                if (medicalOrderDTO.Id != null && pendingInvoiceDict.TryGetValue(medicalOrderDTO.Id, out var pendingInvoice))
                {
                    medicalOrderDTO.PendingInvoiceId = pendingInvoice.Id;
                }
            }
        }

        #region State Machine Management
        public async Task UpdateStatusAsync(string medicalOrderId, ClinicalMedicalOrderStatusEnum clinicalMedicalOrderStatus)
        {
            var clinicalMedicalOrder = await _clinicalMedicalOrderRepository.GetByIdAsync(medicalOrderId)
                ?? throw new DataNotFoundException(typeof(ClinicalMedicalOrder), medicalOrderId);

            this.UpdateStatus(clinicalMedicalOrder, clinicalMedicalOrderStatus);
            await _clinicalMedicalOrderRepository.UpdateAsync(medicalOrderId, clinicalMedicalOrder);

            if (clinicalMedicalOrderStatus == ClinicalMedicalOrderStatusEnum.Cancelled)
            {
                await _messageBus.PublishAsync(new CancelPendingInvoiceByClinicalMedicalOrderIdEvent
                    {
                        ClinicalMedicalOrderId = medicalOrderId
                    });
            }
        }

        public async Task UpdateDetailStatusAsync(
            string medicalOrderId,
            int medicalServiceId,
            ClinicalMedicalOrderDetailStatusEnum clinicalMedicalOrderDetailStatus,
            string? failedReason = null)
        {
            var clinicalMedicalOrder = await _clinicalMedicalOrderRepository.GetByIdAsync(medicalOrderId)
                ?? throw new DataNotFoundException(typeof(ClinicalMedicalOrder), medicalOrderId);
            if (clinicalMedicalOrder.ClinicalMedicalOrderStatus != ClinicalMedicalOrderStatusEnum.Paid)
            {
                throw new InvalidDataException($"Cannot update clinical medical order detail status when clinical medical order status is {clinicalMedicalOrder.ClinicalMedicalOrderStatus}.");
            }

            var clinicalMedicalOrderDetail = clinicalMedicalOrder.ClinicalMedicalOrderDetails
                .FirstOrDefault(detail => detail.MedicalServiceId == medicalServiceId)
                    ?? throw new DataNotFoundException($"Medical Service ({medicalServiceId}) was not found in Clinical Medical Order ({medicalOrderId})!");

            this.UpdateDetailStatus(clinicalMedicalOrderDetail, clinicalMedicalOrderDetailStatus);
            await _clinicalMedicalOrderRepository.UpdateAsync(medicalOrderId, clinicalMedicalOrder);

            if (clinicalMedicalOrderDetailStatus == ClinicalMedicalOrderDetailStatusEnum.Failed)
            {
                var createRefundContract = new CreateRefundContract
                {
                    ClinicalMedicalOrderId = medicalOrderId,
                    MedicalServiceId = clinicalMedicalOrderDetail.MedicalServiceId,
                    Quantity = clinicalMedicalOrderDetail.Quantity,
                    ClinicalMedicalOrderDetailId = clinicalMedicalOrderDetail.Id,
                    Reason = failedReason ?? "No reason provided"
                };

                await _messageBus.PublishAsync<CreateRefundWhenClinicalMedicalOrderFailedEvent>(new()
                {
                    Data = createRefundContract
                });
            }
        }

        private void UpdateStatus(ClinicalMedicalOrder clinicalMedicalOrder, ClinicalMedicalOrderStatusEnum clinicalMedicalOrderStatus)
        {
            var stateMachine = new ClinicalMedicalOrderStatusStateMachine(clinicalMedicalOrder);
            if (!stateMachine.CanFire(clinicalMedicalOrderStatus))
            {
                throw new InvalidDataException(
                    $"Cannot change clinical medical order status from {clinicalMedicalOrder.ClinicalMedicalOrderStatus} to {clinicalMedicalOrderStatus}.");
            }

            try
            {
                stateMachine.Fire(clinicalMedicalOrderStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change clinical medical order status: {e.Message}");
            }
        }

        private void UpdateDetailStatus(ClinicalMedicalOrderDetail clinicalMedicalOrderDetail, ClinicalMedicalOrderDetailStatusEnum clinicalMedicalOrderDetailStatus)
        {
            var stateMachine = new ClinicalMedicalOrderDetailStatusStateMachine(clinicalMedicalOrderDetail);
            if (!stateMachine.CanFire(clinicalMedicalOrderDetailStatus))
            {
                throw new InvalidDataException(
                    $"Cannot change clinical medical order detail status from {clinicalMedicalOrderDetail.ClinicalMedicalOrderDetailStatus} to {clinicalMedicalOrderDetailStatus}.");
            }

            try
            {
                stateMachine.Fire(clinicalMedicalOrderDetailStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change clinical medical order detail status: {e.Message}");
            }
        }
        #endregion
    }
}
