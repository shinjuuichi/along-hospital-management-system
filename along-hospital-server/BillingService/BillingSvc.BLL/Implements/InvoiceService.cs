using AutoMapper;
using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.BLL.DTOs.RefundDTOs;
using BillingSvc.BLL.Interfaces;
using BillingSvc.BLL.StateMachines;
using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.MedicalOrderContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.MedicalHistoryEvents;
using MessageBroker.Events.MedicalOrderEvents;
using MessageBroker.Events.MedicalServiceEvents;
using MessageBroker.Events.PaymentEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;

namespace BillingSvc.BLL.Implements
{
    public class InvoiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
            : BaseService<Invoice, CreateInvoiceDTO, object, GetInvoiceDTO>(
                unitOfWork,
                mapper,
                includes: [$"{nameof(Invoice.Charges)}.{nameof(Charge.Refund)}"]),
            IInvoiceService
    {
        private const string DEFAULT_PAYMENT_TYPE = "PayOS";
        private const string DEFAULT_PAYMENT_STATUS_SUCCESS = "Success";
        private const string DEFAULT_PAYMENT_STATUS_FAILED = "Failed";
        private const string DEFAULT_PAYMENT_STATUS_CANCELLED = "Cancelled";

        private const string MEDICAL_HISTORY_STATUS_DRAFT = "Draft";

        private const string MEDICAL_HISTORY_TYPE_INPATIENT = "Inpatient";
        private const string MEDICAL_HISTORY_TYPE_OUTPATIENT = "Outpatient";

        private readonly IMessageBus _messageBus = messageBus;

        #region Get
        public override async Task<GetInvoiceDTO> GetByIdAsync(int id)
        {
            var invoiceDTO = await base.GetByIdAsync(id);
            await this.RequestValueForInvoiceDTOsAsync([invoiceDTO]);
            return invoiceDTO;
        }

        public override async Task<PaginationResult<GetInvoiceDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var results = await base.GetAllPaginatedAsync(filterDTO);
            await this.RequestValueForInvoiceDTOsAsync(results.Collection);
            return results;
        }

        public async Task<List<GetInvoiceDTO>> GetAllByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var invoices = await _repository.GetAllAsync(i => i.MedicalHistoryId == medicalHistoryId, _includes);
            var invoiceDTOs = _mapper.Map<List<GetInvoiceDTO>>(invoices);
            await this.RequestValueForInvoiceDTOsAsync(invoiceDTOs);
            return invoiceDTOs;
        }

        public async Task<List<GetInvoiceDTO>> GetOrCreateAllPendingInvoicesByClinicalMedicalOrderIdsAsync(Dictionary<string, GetClinicalMedicalOrderContract> clinicalMedicalOrderDict)
        {
            List<GetInvoiceDTO> invoiceDTOs = [];

            var clinicalMedicalOrderIds = clinicalMedicalOrderDict.Keys.ToList();

            var pendingInvoices = await _repository.GetAllAsync(i =>
                !string.IsNullOrEmpty(i.ClinicalMedicalOrderId)
                && clinicalMedicalOrderIds.Contains(i.ClinicalMedicalOrderId)
                && i.InvoiceStatus == InvoiceStatusEnum.Pending);

            var pendingInvoiceDict = pendingInvoices
                .ToDictionary(i => i.ClinicalMedicalOrderId!);

            foreach (var (key, value) in clinicalMedicalOrderDict)
            {
                if (pendingInvoiceDict.TryGetValue(key, out var pendingInvoice))
                {
                    invoiceDTOs.Add(_mapper.Map<GetInvoiceDTO>(pendingInvoice));
                    continue;
                }

                try
                {
                    var createInvoiceDTO = new CreateInvoiceDTO
                    {
                        MedicalHistoryId = value.MedicalHistoryId,
                        ClinicalMedicalOrderId = key,
                        Charges = value.ClinicalMedicalOrderDetails.Select(d => new CreateChargeDTO()
                        {
                            Quantity = d.Quantity,
                            MedicalServiceId = d.MedicalServiceId,
                            ChargeType = nameof(ChargeTypeEnum.Invoice)
                        }).ToList()
                    };

                    var createdInvoiceDTO = await this.CreateAsync(createInvoiceDTO);
                    invoiceDTOs.Add(createdInvoiceDTO);
                }
                catch
                {
                    continue;
                }
            }

            return invoiceDTOs;
        }
        #endregion

        #region Create
        public override async Task<GetInvoiceDTO> CreateAsync(CreateInvoiceDTO createDTO)
        {
            // Get Medical History Info to verify existence and status before creating invoice
            var medicalHistoryContract = await _messageBus.RequestAsync<
                GetMedicalHistoryByIdEvent,
                GetMedicalHistoryContract>(new()
                {
                    Id = createDTO.MedicalHistoryId
                });
            if (medicalHistoryContract.MedicalHistoryStatus != MEDICAL_HISTORY_STATUS_DRAFT)
            {
                throw new InvalidDataException($"Cannot create invoice for medical history with status {medicalHistoryContract.MedicalHistoryStatus}!");
            }

            // Verify at least one charge
            var distinctMedicalServiceIds = createDTO.Charges.Select(su => su.MedicalServiceId)
                .Distinct().ToList();
            if (distinctMedicalServiceIds.Count == 0)
            {
                throw new InvalidDataException("At least one charge is required to create an invoice!");
            }

            // Get Medical Service Info to fill in unit price and snapshot
            var medicalServiceContracts = await _messageBus.RequestAsync<
                    GetListMedicalServiceDataByIdsEvent,
                    GetListMedicalServiceDataContract>(new()
                    {
                        Ids = distinctMedicalServiceIds
                    });

            Dictionary<int, GetMedicalServiceContract> medicalServiceContractDict
                = medicalServiceContracts.Data.ToDictionary(ms => ms.Id);

            foreach (var charge in createDTO.Charges)
            {
                if (!medicalServiceContractDict.TryGetValue(charge.MedicalServiceId, out var medicalServiceContract))
                {
                    throw new DataNotFoundException($"Medical Service ({charge.MedicalServiceId}) was not found!");
                }

                charge.UnitPrice = medicalServiceContract.Price;
                charge.ChargeSnapshot = _mapper.Map<CreateChargeSnapshotDTO>(medicalServiceContract);
                charge.ChargeType = nameof(ChargeTypeEnum.Invoice);
            }

            // Create Invoice
            createDTO.InvoiceNumber = this.GenerateInvoiceNumber(medicalHistoryContract.MedicalHistoryType);
            var invoice = _mapper.Map<Invoice>(createDTO);

            var createdInvoice = await _repository.AddAsync(invoice);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetInvoiceDTO>(createdInvoice);
        }

        public async Task CreateGeneralInvoiceAsync(CreateGeneralInvoiceDTO createGeneralInvoiceDTO)
        {
            var medicalServiceContract = await _messageBus.RequestAsync<
               GetMedicalServiceByCodeEvent,
               GetMedicalServiceContract>(new()
               {
                   Code = MedicalServiceCodeConstants.GENERAL_HEALTH_CHECK_CODE
               });

            createGeneralInvoiceDTO.InvoiceNumber = this.GenerateInvoiceNumber(createGeneralInvoiceDTO.MedicalHistoryType);
            createGeneralInvoiceDTO.Charges = [
                new CreateChargeDTO
                {
                    MedicalServiceId = medicalServiceContract.Id,
                    Quantity = 1,
                    UnitPrice = medicalServiceContract.Price,
                    ChargeSnapshot = _mapper.Map<CreateChargeSnapshotDTO>(medicalServiceContract)
                }
            ];

            var invoice = _mapper.Map<Invoice>(createGeneralInvoiceDTO);
            invoice.InvoiceStatus = createGeneralInvoiceDTO.IsPaid ? InvoiceStatusEnum.Completed : InvoiceStatusEnum.Pending;

            await _repository.AddAsync(invoice);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion

        #region Update Status
        public async Task UpdateStatusAsync(int invoiceId, InvoiceStatusEnum invoiceStatus)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId, _includes);
            if (invoice == null)
            {
                throw new DataNotFoundException(typeof(Invoice), invoiceId);
            }

            this.UpdateStatus(invoice, invoiceStatus);
            _repository.Update(invoice);
            await _unitOfWork.SaveChangeAsync();

            if (invoiceStatus == InvoiceStatusEnum.Completed)
            {
                await this.OnPaymentCompletedAsync(invoice);
            }
        }

        public async Task CancelAllPendingByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var pendingInvoices = await _repository.GetAllAsync(i =>
                i.MedicalHistoryId == medicalHistoryId
                && i.InvoiceStatus == InvoiceStatusEnum.Pending,
                _includes);

            if (pendingInvoices.Count == 0)
            {
                return;
            }

            foreach (var invoice in pendingInvoices)
            {
                this.UpdateStatus(invoice, InvoiceStatusEnum.Cancelled);
            }

            _repository.UpdateRange(pendingInvoices);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task CancelPendingByClinicalMedicalOrderIdAsync(string clinicalMedicalOrderId)
        {
            var pendingInvoices = await _repository.GetAllAsync(i =>
                i.ClinicalMedicalOrderId == clinicalMedicalOrderId
                && i.InvoiceStatus == InvoiceStatusEnum.Pending,
                _includes);

            if (pendingInvoices.Count == 0)
            {
                return;
            }

            foreach (var invoice in pendingInvoices)
            {
                this.UpdateStatus(invoice, InvoiceStatusEnum.Cancelled);
            }

            _repository.UpdateRange(pendingInvoices);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task CancelPendingByClinicalMedicalOrderIdsAsync(List<string> clinicalMedicalOrderIds)
        {
            if (clinicalMedicalOrderIds.Count == 0)
            {
                return;
            }

            var validClinicalMedicalOrderIds = clinicalMedicalOrderIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            if (validClinicalMedicalOrderIds.Count == 0)
            {
                return;
            }

            var pendingInvoices = await _repository.GetAllAsync(i =>
                !string.IsNullOrEmpty(i.ClinicalMedicalOrderId)
                && validClinicalMedicalOrderIds.Contains(i.ClinicalMedicalOrderId)
                && i.InvoiceStatus == InvoiceStatusEnum.Pending,
                _includes);

            if (pendingInvoices.Count == 0)
            {
                return;
            }

            foreach (var invoice in pendingInvoices)
            {
                this.UpdateStatus(invoice, InvoiceStatusEnum.Cancelled);
            }

            _repository.UpdateRange(pendingInvoices);
            await _unitOfWork.SaveChangeAsync();
        }

        private void UpdateStatus(Invoice invoice, InvoiceStatusEnum invoiceStatus)
        {
            var invoiceStatusStateMachine = new InvoiceStatusStateMachine(invoice);
            if (!invoiceStatusStateMachine.CanFire(invoiceStatus))
            {
                throw new InvalidDataException($"Cannot change invoice status from {invoice.InvoiceStatus} to {invoiceStatus}");
            }
            try
            {
                invoiceStatusStateMachine.Fire(invoiceStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change invoice status: {e.Message}");
            }
        }
        #endregion

        #region Payment Handling
        public async Task<string> GetPaymentUrlByInvoiceIdAsync(int invoiceId)
        {
            var invoice = await _repository.GetByIdAsync(invoiceId, _includes);
            if (invoice == null)
            {
                throw new DataNotFoundException(typeof(Invoice), invoiceId);
            }

            if (invoice.InvoiceStatus != InvoiceStatusEnum.Pending)
            {
                throw new InvalidDataException($"Cannot get payment url for invoice with status {invoice.InvoiceStatus}!");
            }

            if (invoice.TransactionId == null)
            {
                return await this.CreatePaymentUrlForInvoiceAsync(invoice);
            }

            var getPaymentUrlContract = await _messageBus.RequestAsync<
                CheckPaymentExistByTransactionIdEvent,
                CheckPaymentExistByTransactionIdContract>(new()
                {
                    TransactionId = invoice.TransactionId.Value
                });

            if (string.IsNullOrEmpty(getPaymentUrlContract.PaymentUrl))
            {
                return await this.CreatePaymentUrlForInvoiceAsync(invoice);
            }

            return getPaymentUrlContract.PaymentUrl;
        }

        public async Task HandlePaymentStatusChangedAsync(PaymentStatusChangedDTO paymentStatusChangedDTO)
        {
            var invoice = await _repository.GetByConditionAsync(
                i => i.TransactionId == paymentStatusChangedDTO.TransactionId, _includes)
                ?? throw new DataNotFoundException($"{nameof(Invoice)} with TransactionId {paymentStatusChangedDTO.TransactionId} was not found!");

            if (invoice.InvoiceStatus != InvoiceStatusEnum.Pending)
            {
                throw new InvalidDataException($"Cannot change payment status for invoice with status {invoice.InvoiceStatus}!");
            }

            switch (paymentStatusChangedDTO.PaymentStatus)
            {
                case DEFAULT_PAYMENT_STATUS_SUCCESS:
                    await this.UpdateStatusAsync(invoice.Id, InvoiceStatusEnum.Completed);
                    break;

                case DEFAULT_PAYMENT_STATUS_FAILED:
                case DEFAULT_PAYMENT_STATUS_CANCELLED:
                    invoice.TransactionId = null;

                    _repository.Update(invoice);
                    await _unitOfWork.SaveChangeAsync();
                    break;
            }
        }

        private async Task OnPaymentCompletedAsync(Invoice invoice)
        {
            if (invoice.ClinicalMedicalOrderId != null)
            {
                var updateMedicalOrderWhenInvoiceCompletedEvent = new UpdateMedicalOrderWhenInvoiceCompletedEvent
                {
                    Id = invoice.ClinicalMedicalOrderId
                };
                await _messageBus.PublishAsync(updateMedicalOrderWhenInvoiceCompletedEvent);
            }
            else if (invoice.Charges.Count == 1)
            {
                var charge = invoice.Charges.First();
                if (charge.ChargeSnapshot?.MedicalServiceCode == MedicalServiceCodeConstants.GENERAL_HEALTH_CHECK_CODE)
                {
                    var draftMedicalHistoryWhenGeneralPaymentCompletedEvent = new DraftMedicalHistoryWhenGeneralPaymentCompletedEvent
                    {
                        Id = invoice.MedicalHistoryId
                    };
                    await _messageBus.PublishAsync(draftMedicalHistoryWhenGeneralPaymentCompletedEvent);
                }
            }
        }
        #endregion

        #region Helpers
        private async Task<string> CreatePaymentUrlForInvoiceAsync(Invoice invoice)
        {
            var invoiceCharges = invoice.Charges.Where(c => c.ChargeType == ChargeTypeEnum.Invoice).ToList();
            var createPaymentEvent = new CreatePaymentEvent
            {
                Provider = DEFAULT_PAYMENT_TYPE,
                Description = $"{invoice.InvoiceNumber}",
                PaymentEventItems = _mapper.Map<List<PaymentEventItem>>(invoiceCharges),
            };

            var createPaymentContract = await _messageBus.RequestAsync<
                CreatePaymentEvent,
                CreatePaymentContract>(createPaymentEvent);
            if (createPaymentContract == null || string.IsNullOrEmpty(createPaymentContract.PaymentUrl))
            {
                throw new InvalidDataException("Failed to create payment url");
            }

            invoice.TransactionId = createPaymentContract.TransactionId;

            _repository.Update(invoice);
            await _unitOfWork.SaveChangeAsync();

            return createPaymentContract.PaymentUrl;
        }

        private async Task RequestValueForInvoiceDTOsAsync(List<GetInvoiceDTO> invoiceDTOs)
        {
            // Select distinct IDs
            var approvedStaffIds = invoiceDTOs.SelectMany(i => i.Charges)
                .Where(c => c.Refund != null && c.Refund.ApprovedBy.HasValue)
                .Select(c => c.Refund!.ApprovedBy!.Value)
                .Distinct().ToList();

            // Request data and convert to dictionary
            Dictionary<int, GetStaffDataByUserIdContract> staffDict = [];

            if (approvedStaffIds.Count > 0)
            {
                var staffDataContracts = await _messageBus.RequestAsync<
                    GetListStaffDataByUserIdsEvent,
                    GetListStaffDataByUserIdsContract>(new()
                    {
                        UserIds = approvedStaffIds
                    });

                staffDict = staffDataContracts.Data.ToDictionary(s => s.UserId);
            }

            // Check if any dictionary has value to avoid unnecessary looping
            var allDictHasValue = staffDict.Any();
            if (!allDictHasValue)
            {
                return;
            }

            // Fill in data
            foreach (var invoiceDTO in invoiceDTOs)
            {
                foreach (var chargeDTO in invoiceDTO.Charges)
                {
                    if (chargeDTO.Refund != null && chargeDTO.Refund.ApprovedBy.HasValue)
                    {
                        var approvedBy = chargeDTO.Refund.ApprovedBy.Value;
                        if (staffDict.TryGetValue(approvedBy, out var staffData))
                        {
                            chargeDTO.Refund.Staff = _mapper.Map<GetRefundStaffDTO>(staffData);
                        }
                    }
                }
            }
        }

        /*
         * Generate Invoice Number with format: INV-{TypeCode}-{DateCode}-{TicksCode}
         * TypeCode: IPD for Inpatient, OPD for Outpatient, OTH for others
         * DateCode: yyyyMMdd
         * TicksCode: last 6 digits of ticks to ensure uniqueness
         */
        private string GenerateInvoiceNumber(string? medicalHistoryType)
        {
            var dateTimeNow = DateTime.UtcNow;

            var prefix = "INV";
            var typeCode = medicalHistoryType switch
            {
                MEDICAL_HISTORY_TYPE_INPATIENT => "IPD",
                MEDICAL_HISTORY_TYPE_OUTPATIENT => "OPD",
                _ => "OTH"
            };
            var dateCode = dateTimeNow.ToString("yyyyMMdd");
            var ticksCode = (dateTimeNow.Ticks % 1000000).ToString("D6");

            return $"{prefix}-{typeCode}-{dateCode}-{ticksCode}";
        }
        #endregion
    }
}
