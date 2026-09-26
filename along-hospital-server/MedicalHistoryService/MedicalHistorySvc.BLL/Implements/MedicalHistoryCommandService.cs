using AutoMapper;
using MedicalHistorySvc.BLL.Commons;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.BLL.StateMachines;
using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using MedicalHistorySvc.DAL.Models.Snapshots;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.BillingEvents;
using MessageBroker.Events.InPatientResourceEvents;
using MessageBroker.Events.MedicalServiceEvents;
using MessageBroker.Events.MedicalOrderEvents;
using MessageBroker.Events.PatientEvents;
using MessageBroker.Events.StaffEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;

namespace MedicalHistorySvc.BLL.Implements
{
    public class MedicalHistoryCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
            : IMedicalHistoryCommandService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        private readonly IGenericRepository<MedicalHistory> _medicalHistoryRepository = unitOfWork.Repository<MedicalHistory>();

        private const string ACTIVE_OCCUPANCY_STATUS = "Active";
        private const string INVOICE_STATUS_PENDING = "Pending";
        private const string INVOICE_STATUS_COMPLETED = "Completed";

        #region Create
        public async Task<GetMedicalHistoryDTO> CreateAsync(CreateMedicalHistoryDTO createMedicalHistoryDTO)
        {
            if (!Enum.TryParse<MedicalHistoryTypeEnum>(createMedicalHistoryDTO.MedicalHistoryType, out var medicalHistoryType))
            {
                throw new InvalidDataException("Invalid medical history type");
            }

            await _messageBus.RequestAsync<CheckPatientExistByIdEvent, CheckPatientExistByIdContract>(new() { PatientId = createMedicalHistoryDTO.PatientId });
            await _messageBus.RequestAsync<CheckSpecialtyExistByIdEvent, CheckSpecialtyExistByIdContract>(new() { SpecialtyId = createMedicalHistoryDTO.SpecialtyId });

            if (medicalHistoryType == MedicalHistoryTypeEnum.Inpatient)
            {
                if (createMedicalHistoryDTO.AssignedDoctorId <= 0)
                {
                    throw new InvalidDataException("Assigned doctor is required for inpatient medical history");
                }

                await _messageBus.RequestAsync<CheckStaffExistByIdEvent, CheckStaffExistByIdContract>(new() { StaffId = createMedicalHistoryDTO.AssignedDoctorId });
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                createMedicalHistoryDTO.MedicalHistoryNumber = this.GenerateMedicalHistoryNumber(medicalHistoryType);

                var medicalHistory = _mapper.Map<MedicalHistory>(createMedicalHistoryDTO);
                if (medicalHistoryType == MedicalHistoryTypeEnum.Outpatient)
                {
                    medicalHistory.MedicalHistoryStatus = createMedicalHistoryDTO.IsCreatedFromAppointment
                        ? MedicalHistoryStatusEnum.Draft
                        : MedicalHistoryStatusEnum.PendingPayment;
                }
                else if (medicalHistoryType == MedicalHistoryTypeEnum.Inpatient)
                {
                    medicalHistory.MedicalHistoryStatus = MedicalHistoryStatusEnum.Draft;
                    medicalHistory.DoctorId = createMedicalHistoryDTO.AssignedDoctorId;
                }

                var createdMedicalHistory = await _medicalHistoryRepository.AddAsync(medicalHistory);
                await _unitOfWork.SaveChangeAsync();

                if (medicalHistoryType == MedicalHistoryTypeEnum.Outpatient)
                {
                    await _messageBus.RequestAsync<
                        CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedEvent,
                        CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedContract>(new()
                        {
                            MedicalHistoryId = createdMedicalHistory.Id,
                            MedicalHistoryType = medicalHistoryType.ToString(),
                            IsPaid = createMedicalHistoryDTO.IsCreatedFromAppointment
                        });
                }

                await _unitOfWork.CommitTransactionAsync();

                var medicalHistoryDTO = _mapper.Map<GetMedicalHistoryDTO>(medicalHistory);
                return medicalHistoryDTO;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        #endregion

        #region Update
        public async Task AssignDoctorToMedicalHistoryAsync(int medicalHistoryId, int doctorId)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(medicalHistoryId)
                ?? throw new DataNotFoundException(typeof(MedicalHistory), medicalHistoryId);
            await _messageBus.RequestAsync<CheckStaffExistByIdEvent, CheckStaffExistByIdContract>(new() { StaffId = doctorId });

            if (medicalHistory.DoctorId != null)
            {
                throw new DataConflictException($"Medical history already has a doctor assigned");
            }

            medicalHistory.DoctorId = doctorId;
            _medicalHistoryRepository.Update(medicalHistory);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateAsync(int id, UpdateMedicalHistoryDTO updateMedicalHistoryDTO)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(id);
            if (medicalHistory == null)
            {
                throw new DataNotFoundException(typeof(MedicalHistory), id);
            }

            if (medicalHistory.DoctorId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this medical history");
            }

            if (medicalHistory.MedicalHistoryStatus != MedicalHistoryStatusEnum.Draft)
            {
                throw new InvalidDataException("Only medical history with Draft status can be updated");
            }

            medicalHistory = _mapper.Map(updateMedicalHistoryDTO, medicalHistory);
            _medicalHistoryRepository.Update(medicalHistory);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DischargeInpatientBedAsync(int id)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(MedicalHistory), id);

            if (medicalHistory.MedicalHistoryType != MedicalHistoryTypeEnum.Inpatient)
            {
                throw new InvalidDataException("Only inpatient medical history can discharge bed");
            }

            if (medicalHistory.MedicalHistoryStatus != MedicalHistoryStatusEnum.Draft)
            {
                throw new InvalidDataException("Only draft inpatient medical history can discharge bed");
            }

            var bedOccupancies = await this.GetBedOccupanciesByMedicalHistoryIdAsync(id);
            if (!this.HasActiveBedOccupancy(bedOccupancies))
            {
                if (bedOccupancies.Count == 0)
                {
                    throw new ValidationFailureException(
                        "Patient must have an active bed assignment before discharge");
                }

                return;
            }

            var bedChargeInvoiceDTO = await this.BuildBedChargeInvoiceDTOAsync(
                id,
                bedOccupancies);
            if (bedChargeInvoiceDTO.Charges.Count > 0)
            {
                await _messageBus.RequestAsync<
                    CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent,
                    CreateBedChargeInvoiceWhenMedicalHistoryDischargedContract>(
                        _mapper.Map<CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent>(
                            bedChargeInvoiceDTO));
            }

            await _messageBus.RequestAsync<
                DischargeBedOccupancyByMedicalHistoryIdEvent,
                DischargeBedOccupancyByMedicalHistoryIdContract>(new()
                {
                    MedicalHistoryId = id
                });
        }
        #endregion

        #region State Machine Management
        public async Task UpdateStatusAsync(int id, MedicalHistoryStatusEnum medicalHistoryStatus, string functionSource = FunctionSourceConstants.FROM_API)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(id);
            if (medicalHistory == null)
            {
                throw new DataNotFoundException(typeof(MedicalHistory), id);
            }

            if (functionSource == FunctionSourceConstants.FROM_API
                && medicalHistory.DoctorId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to complete this medical history");
            }

            if (medicalHistoryStatus is MedicalHistoryStatusEnum.Completed or MedicalHistoryStatusEnum.Cancelled)
            {
                await this.EnsureBedDischargedBeforeFinalizingAsync(medicalHistory, medicalHistoryStatus);
            }

            switch (medicalHistoryStatus)
            {
                case MedicalHistoryStatusEnum.Completed:
                    var getInvoicesForCompleteContract = await _messageBus.RequestAsync<
                        GetListInvoiceDataByMedicalHistoryIdEvent,
                        GetListInvoiceDataContract>(new() { MedicalHistoryId = id });

                    if (getInvoicesForCompleteContract.Data.Any(invoice => invoice.InvoiceStatus == INVOICE_STATUS_PENDING))
                    {
                        throw new InvalidDataException("Cannot complete medical history with pending invoices");
                    }

                    var getPatientDataByUserIdEvent = new GetPatientDataByUserIdEvent { UserId = medicalHistory.PatientId };
                    var getPatientDataByUserIdContract = await _messageBus
                        .RequestAsync<GetPatientDataByUserIdEvent, GetPatientDataByUserIdContract>(getPatientDataByUserIdEvent);

                    medicalHistory.PatientSnapshot = _mapper.Map<PatientSnapshot>(getPatientDataByUserIdContract);
                    break;

                case MedicalHistoryStatusEnum.Cancelled:
                    var getAllInvoicesByMedicalHistoryIdContract = await _messageBus.RequestAsync<
                    GetListInvoiceDataByMedicalHistoryIdEvent,
                    GetListInvoiceDataContract>(new() { MedicalHistoryId = id });

                    if (getAllInvoicesByMedicalHistoryIdContract.Data.Any(invoice => invoice.InvoiceStatus == INVOICE_STATUS_COMPLETED))
                    {
                        throw new InvalidDataException("Cannot cancel medical history with completed invoices");
                    }
                    break;
            }

            this.UpdateStatus(medicalHistory, medicalHistoryStatus);
            _medicalHistoryRepository.Update(medicalHistory);
            await _unitOfWork.SaveChangeAsync();

            if (medicalHistoryStatus == MedicalHistoryStatusEnum.Cancelled)
            {
                await _messageBus.PublishAsync(new CancelPendingInvoicesByMedicalHistoryIdEvent
                {
                    MedicalHistoryId = id
                });

                await _messageBus.RequestAsync<
                    CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdEvent,
                    CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdContract>(new()
                    {
                        MedicalHistoryId = id
                    });
            }
        }

        private void UpdateStatus(MedicalHistory medicalHistory, MedicalHistoryStatusEnum medicalHistoryStatus)
        {
            var medicalHistoryStateMachine = new MedicalHistoryStateMachine(medicalHistory);
            if (!medicalHistoryStateMachine.CanFire(medicalHistoryStatus))
            {
                throw new InvalidDataException($"Cannot change medical history status from {medicalHistory.MedicalHistoryStatus} to {medicalHistoryStatus}");
            }

            try
            {
                medicalHistoryStateMachine.Fire(medicalHistoryStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change medical history status: {e.Message}");
            }
        }

        private async Task EnsureBedDischargedBeforeFinalizingAsync(
            MedicalHistory medicalHistory,
            MedicalHistoryStatusEnum targetStatus)
        {
            if (medicalHistory.MedicalHistoryType != MedicalHistoryTypeEnum.Inpatient)
            {
                return;
            }

            var bedOccupancies = await this.GetBedOccupanciesByMedicalHistoryIdAsync(medicalHistory.Id);
            if (this.HasActiveBedOccupancy(bedOccupancies))
            {
                var action = targetStatus == MedicalHistoryStatusEnum.Cancelled
                    ? "cancel"
                    : "complete";

                throw new InvalidDataException(
                    $"Cannot {action} inpatient medical history before discharging the bed");
            }
        }
        #endregion

        #region Background Jobs
        public async Task CancelPendingPaymentMedicalHistoriesAsync()
        {
            var nowDateTime = DateTime.UtcNow;
            var expiredPaymentDays = 7;

            var pendingPaymentMedicalHistories = await _medicalHistoryRepository.GetAllAsync(
                mh => mh.MedicalHistoryStatus == MedicalHistoryStatusEnum.PendingPayment
                    && mh.AdmissionDate.AddDays(expiredPaymentDays) < nowDateTime);

            List<MedicalHistory> successfullyCancelledMedicalHistories = [];

            foreach (var medicalHistory in pendingPaymentMedicalHistories)
            {
                try
                {
                    this.UpdateStatus(medicalHistory, MedicalHistoryStatusEnum.Cancelled);
                    successfullyCancelledMedicalHistories.Add(medicalHistory);
                }
                catch
                {
                    continue;
                }
            }

            if (successfullyCancelledMedicalHistories.Count > 0)
            {
                _medicalHistoryRepository.UpdateRange(successfullyCancelledMedicalHistories);
                await _unitOfWork.SaveChangeAsync();
            }
        }
        #endregion

        #region Private functions
        /*
         * Generate medical history number with format: MH-{TypeCode}-{DateCode}-{TicksCode}
         * TypeCode: OP for Outpatient, IP for Inpatient
         * DateCode: yyyyMMdd
         * TicksCode: last 6 digits of ticks to ensure uniqueness
         */
        private string GenerateMedicalHistoryNumber(MedicalHistoryTypeEnum medicalHistoryType)
        {
            var dateTimeNow = DateTime.UtcNow;

            var prefix = "MH";
            var typeCode = medicalHistoryType == MedicalHistoryTypeEnum.Outpatient ? "OP" : "IP";
            var dateCode = dateTimeNow.ToString("yyyyMMdd");
            var ticksCode = (dateTimeNow.Ticks % 1000000).ToString("D6");

            return $"{prefix}-{typeCode}-{dateCode}-{ticksCode}";
        }

        private async Task<List<GetBedOccupancyByMedicalHistoryIdContract>> GetBedOccupanciesByMedicalHistoryIdAsync(
            int medicalHistoryId)
        {
            var bedOccupancyContract = await _messageBus.RequestAsync<
                GetListBedOccupancyDataByMedicalHistoryIdEvent,
                GetListBedOccupancyDataContract>(new()
                {
                    MedicalHistoryId = medicalHistoryId
                });

            return bedOccupancyContract.Data
                .OrderBy(occupancy => occupancy.FromDateTime)
                .ThenBy(occupancy => occupancy.Id)
                .ToList();
        }

        private bool HasActiveBedOccupancy(List<GetBedOccupancyByMedicalHistoryIdContract> bedOccupancies)
        {
            return bedOccupancies.Any(occupancy =>
                string.Equals(
                    occupancy.OccupancyStatus,
                    ACTIVE_OCCUPANCY_STATUS));
        }

        private async Task<CreateBedChargeInvoiceWhenMedicalHistoryDischargedDTO> BuildBedChargeInvoiceDTOAsync(
            int medicalHistoryId,
            List<GetBedOccupancyByMedicalHistoryIdContract> bedOccupancies)
        {
            List<(string MedicalServiceCode, int Quantity)> chargeItemsByCode = [];

            foreach (var bedOccupancy in bedOccupancies)
            {
                if (bedOccupancy.Bed == null)
                {
                    throw new DataNotFoundException(
                        $"Bed information was not found for bed occupancy {bedOccupancy.Id}");
                }

                if (string.IsNullOrWhiteSpace(bedOccupancy.Bed.BedCategoryCode))
                {
                    throw new DataNotFoundException(
                        $"Bed category code was not found for bed occupancy {bedOccupancy.Id}");
                }

                var effectiveToDateTime = bedOccupancy.ToDateTime;
                if (!effectiveToDateTime.HasValue
                    && string.Equals(
                        bedOccupancy.OccupancyStatus,
                        ACTIVE_OCCUPANCY_STATUS,
                        StringComparison.OrdinalIgnoreCase))
                {
                    effectiveToDateTime = DateTime.UtcNow;
                }

                var quantity = this.CalculateOccupancyChargeQuantity(
                    bedOccupancy.FromDateTime,
                    effectiveToDateTime);
                if (quantity == 0)
                {
                    continue;
                }

                chargeItemsByCode.Add((bedOccupancy.Bed.BedCategoryCode, quantity));
            }

            if (chargeItemsByCode.Count == 0)
            {
                return new CreateBedChargeInvoiceWhenMedicalHistoryDischargedDTO
                {
                    MedicalHistoryId = medicalHistoryId
                };
            }

            var medicalServiceContracts = await _messageBus.RequestAsync<
                GetListMedicalServiceDataByCodesEvent,
                GetListMedicalServiceDataContract>(new()
                {
                    Codes = chargeItemsByCode
                        .Select(chargeItem => chargeItem.MedicalServiceCode)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList()
                });

            var medicalServiceContractDict = medicalServiceContracts.Data
                .Where(contract => !string.IsNullOrWhiteSpace(contract.Code))
                .ToDictionary(contract => contract.Code!, StringComparer.OrdinalIgnoreCase);

            return new CreateBedChargeInvoiceWhenMedicalHistoryDischargedDTO
            {
                MedicalHistoryId = medicalHistoryId,
                Charges = chargeItemsByCode
                    .Select(chargeItem =>
                    {
                        if (!medicalServiceContractDict.TryGetValue(
                            chargeItem.MedicalServiceCode,
                            out var medicalServiceContract))
                        {
                            throw new DataNotFoundException(
                            $"Medical Service code ({chargeItem.MedicalServiceCode}) was not found!");
                        }

                        return new CreateBedChargeInvoiceWhenMedicalHistoryDischargedChargeItemDTO
                        {
                            MedicalServiceId = medicalServiceContract.Id,
                            Quantity = chargeItem.Quantity
                        };
                    })
                    .ToList()
            };
        }

        private int CalculateOccupancyChargeQuantity(DateTime fromDateTime, DateTime? toDateTime)
        {
            var effectiveToDateTime = toDateTime ?? DateTime.UtcNow;
            if (effectiveToDateTime <= fromDateTime)
            {
                return 0;
            }

            return (int)Math.Ceiling((effectiveToDateTime - fromDateTime).TotalDays);
        }
        #endregion
    }
}
