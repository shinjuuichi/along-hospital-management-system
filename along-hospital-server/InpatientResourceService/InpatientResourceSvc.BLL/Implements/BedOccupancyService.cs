using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.BLL.StateMachines;
using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using MessageBroker.Events.MedicalServiceEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace InpatientResourceSvc.BLL.Implements
{
    public class BedOccupancyService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : IBedOccupancyService
    {
        private const string MEDICAL_HISTORY_TYPE_INPATIENT = "Inpatient";
        private const string MEDICAL_HISTORY_STATUS_DRAFT = "Draft";
        private const string WORK_SCHEDULE_LOCATION_TYPE_ROOM = "Room";

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<BedOccupancy> _bedOccupancyRepository = unitOfWork.Repository<BedOccupancy>();
        private readonly IGenericRepository<Bed> _bedRepository = unitOfWork.Repository<Bed>();
        private readonly IGenericRepository<Room> _roomRepository = unitOfWork.Repository<Room>();
        private readonly string[] _includesAll = [
            $"{nameof(BedOccupancy.Bed)}.{nameof(Bed.BedCategory)}",
            $"{nameof(BedOccupancy.Bed)}.{nameof(Bed.Room)}.{nameof(Room.Floor)}.{nameof(Floor.Building)}"
        ];
        private readonly string[] _roomBoardIncludes = [
            $"{nameof(Room.Floor)}.{nameof(Floor.Building)}",
            $"{nameof(Room.Beds)}.{nameof(Bed.BedCategory)}"
        ];

        public async Task AssignBedAsync(AssignBedDTO assignBedDTO)
        {
            var medicalHistoryContract = await this.ValidateMedicalHistoryForOccupancyManagementAsync(
                assignBedDTO.MedicalHistoryId);

            await this.ValidateMedicalHistoryHasNoActiveOccupancyAsync(assignBedDTO.MedicalHistoryId);

            var bed = await this.GetValidatedBedForOccupancyAsync(
                assignBedDTO.BedId,
                medicalHistoryContract.SpecialtyId);

            var bedOccupancy = _mapper.Map<BedOccupancy>(assignBedDTO);
            bedOccupancy.OccupancyStatus = OccupancyStatusEnum.Active;

            await _bedOccupancyRepository.AddAsync(bedOccupancy);

            bed.Status = BedStatusEnum.Occupied;
            _bedRepository.Update(bed);

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task TransferBedAsync(TransferBedDTO transferBedDTO)
        {
            var medicalHistoryContract = await this.ValidateMedicalHistoryForOccupancyManagementAsync(
                transferBedDTO.MedicalHistoryId);

            if (string.IsNullOrWhiteSpace(transferBedDTO.TransferNote))
            {
                throw new ValidationFailureException("Transfer note is required");
            }

            var activeOccupancy = await this.GetActiveOccupancyByMedicalHistoryIdAsync(transferBedDTO.MedicalHistoryId)
                ?? throw new ValidationFailureException("Patient must have an active bed assignment before transfer");

            if (activeOccupancy.BedId == transferBedDTO.BedId)
            {
                throw new ValidationFailureException("Destination bed must be different from current bed");
            }

            var destinationBed = await this.GetValidatedBedForOccupancyAsync(
                transferBedDTO.BedId,
                medicalHistoryContract.SpecialtyId);

            var sourceBed = await _bedRepository.GetByIdAsync(activeOccupancy.BedId)
                ?? throw new DataNotFoundException(typeof(Bed), activeOccupancy.BedId);

            this.UpdateOccupancyStatus(activeOccupancy, OccupancyStatusEnum.Transferred);
            activeOccupancy.ToDateTime = DateTime.UtcNow;
            activeOccupancy.TransferNote = transferBedDTO.TransferNote.Trim();
            _bedOccupancyRepository.Update(activeOccupancy);

            sourceBed.Status = BedStatusEnum.Active;
            _bedRepository.Update(sourceBed);

            var nextOccupancy = this.CreateActiveBedOccupancy(
                transferBedDTO.MedicalHistoryId,
                transferBedDTO.BedId);

            await _bedOccupancyRepository.AddAsync(nextOccupancy);

            destinationBed.Status = BedStatusEnum.Occupied;
            _bedRepository.Update(destinationBed);

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DischargeByMedicalHistoryIdAsync(
            int medicalHistoryId)
        {
            var activeOccupancy = await this.GetActiveOccupancyByMedicalHistoryIdAsync(medicalHistoryId)
                ?? throw new ValidationFailureException(
                    "Patient must have an active bed assignment before discharge");

            this.UpdateOccupancyStatus(activeOccupancy, OccupancyStatusEnum.Discharged);
            activeOccupancy.ToDateTime = DateTime.UtcNow;
            _bedOccupancyRepository.Update(activeOccupancy);

            var bed = await _bedRepository.GetByIdAsync(activeOccupancy.BedId)
                ?? throw new DataNotFoundException(typeof(Bed), activeOccupancy.BedId);

            bed.Status = BedStatusEnum.Active;
            _bedRepository.Update(bed);

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<GetBedOccupancyDTO?> GetByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var bedOccupancies = await this.GetAllByMedicalHistoryIdAsync(medicalHistoryId);
            if (bedOccupancies.Count == 0)
            {
                return null;
            }

            return bedOccupancies
                .OrderByDescending(occupancy => occupancy.OccupancyStatus == OccupancyStatusEnum.Active.ToString())
                .ThenByDescending(occupancy => occupancy.FromDateTime)
                .First();
        }

        public async Task<List<GetBedOccupancyDTO>> GetAllByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var bedOccupancies = await _bedOccupancyRepository.GetAllAsync(
                occupancy => occupancy.MedicalHistoryId == medicalHistoryId,
                _includesAll);

            if (bedOccupancies.Count == 0)
            {
                return [];
            }

            var orderedBedOccupancies = bedOccupancies
                .OrderBy(occupancy => occupancy.FromDateTime)
                .ThenBy(occupancy => occupancy.Id)
                .ToList();
            var latestTransferNote = this.GetLatestTransferNote(orderedBedOccupancies);
            var roomBedCountDict = await this.GetRoomBedCountDictionaryAsync(orderedBedOccupancies);
            var bedCategoryPriceDict = await this.GetBedCategoryPriceDictionaryAsync(orderedBedOccupancies);

            return orderedBedOccupancies
                .Select(occupancy =>
                {
                    var dto = _mapper.Map<GetBedOccupancyDTO>(occupancy);
                    dto.TransferNote = occupancy.TransferNote?.Trim();
                    dto.LatestTransferNote = latestTransferNote;
                    this.ApplyRoomBedCounts(dto, occupancy.Bed?.RoomId, roomBedCountDict);
                    this.ApplyOccupancyPricing(dto, occupancy, bedCategoryPriceDict);
                    return dto;
                })
                .ToList();
        }

        public async Task<List<GetBedOccupancyBoardRoomDTO>> GetRoomBoardAsync()
        {
            var allowedRoomIds = await this.GetAllowedRoomIdsForCurrentUserAsync();
            if (_currentUserService.Role == RoleEnum.Nurse && allowedRoomIds.Count == 0)
            {
                return [];
            }

            var rooms = await _roomRepository.GetAllAsync(
                room => room.Status == RoomStatusEnum.Active
                    && (_currentUserService.Role != RoleEnum.Nurse || allowedRoomIds.Contains(room.Id)),
                _roomBoardIncludes);

            var bedIds = rooms
                .SelectMany(room => room.Beds)
                .Select(bed => bed.Id)
                .Distinct()
                .ToList();
            var activeOccupancyDict = await this.GetActiveOccupancyDictionaryByBedIdAsync(bedIds);

            var medicalHistorySummaryDict = await this.GetMedicalHistorySummaryDictionaryAsync(
                activeOccupancyDict.Values.Select(occupancy => occupancy.MedicalHistoryId).Distinct().ToList());

            var roomBoard = rooms
                .OrderBy(room => room.Code)
                .Select(room => new GetBedOccupancyBoardRoomDTO
                {
                    Id = room.Id,
                    Code = room.Code,
                    BuildingName = room.Floor?.Building?.Name,
                    FloorNumber = room.Floor?.FloorNumber ?? 0,
                    SpecialtyId = room.SpecialtyId,
                    TotalBeds = room.Beds.Count,
                    OccupiedBeds = room.Beds.Count(bed => bed.Status == BedStatusEnum.Occupied),
                    AvailableBeds = room.Beds.Count(bed => bed.Status == BedStatusEnum.Active),
                    MaintenanceBeds = room.Beds.Count(bed => bed.Status == BedStatusEnum.Maintenance),
                    Beds = room.Beds
                        .OrderBy(bed => bed.Code)
                        .Select(bed =>
                        {
                            activeOccupancyDict.TryGetValue(bed.Id, out var currentOccupancy);

                            return new GetBedOccupancyBoardBedDTO
                            {
                                Id = bed.Id,
                                Code = bed.Code,
                                Status = bed.Status.ToString(),
                                BedCategoryId = bed.BedCategoryId,
                                BedCategoryName = bed.BedCategory?.Name,
                                CurrentOccupancy = currentOccupancy == null
                                    ? null
                                    : this.BuildOccupancySummaryDTO(currentOccupancy, medicalHistorySummaryDict)
                            };
                        })
                        .ToList()
                })
                .ToList();

            await this.RequestValueForRoomBoardAsync(roomBoard);

            return roomBoard;
        }

        #region Helper Methods
        private async Task<List<int>> GetAllowedRoomIdsForCurrentUserAsync()
        {
            if (_currentUserService.Role != RoleEnum.Nurse)
            {
                return [];
            }

            var workDate = DateOnly.FromDateTime(DateTime.UtcNow.ConvertTimeToTimeZone());
            var workScheduleContract = await _messageBus.RequestAsync<
                GetListWorkScheduleByWorkDateEvent,
                GetListWorkScheduleContract>(
                    new GetListWorkScheduleByWorkDateEvent
                    {
                        WorkDate = workDate
                    });

            return workScheduleContract.Data
                .SelectMany(workSchedule => workSchedule.WorkScheduleAssignments)
                .Where(assignment =>
                    assignment.StaffId == _currentUserService.UserId
                    && string.Equals(
                        assignment.LocationType,
                        WORK_SCHEDULE_LOCATION_TYPE_ROOM,
                        StringComparison.OrdinalIgnoreCase))
                .Select(assignment => assignment.LocationId)
                .Distinct()
                .ToList();
        }

        private async Task<GetMedicalHistoryContract> ValidateMedicalHistoryForOccupancyManagementAsync(
            int medicalHistoryId)
        {
            var medicalHistoryContract = await _messageBus.RequestAsync<
                GetMedicalHistoryByIdEvent,
                GetMedicalHistoryContract>(
                    new GetMedicalHistoryByIdEvent
                    {
                        Id = medicalHistoryId
                    });

            if (medicalHistoryContract.MedicalHistoryType != MEDICAL_HISTORY_TYPE_INPATIENT)
            {
                throw new ValidationFailureException(
                    "Medical history must be Inpatient type to manage bed occupancy");
            }

            if (medicalHistoryContract.MedicalHistoryStatus != MEDICAL_HISTORY_STATUS_DRAFT)
            {
                throw new ValidationFailureException(
                    "Medical history must be in Draft status to manage bed occupancy");
            }

            if (medicalHistoryContract.DischargeDate.HasValue)
            {
                throw new ValidationFailureException(
                    "Bed flow is closed because this medical history has already been discharged");
            }

            return medicalHistoryContract;
        }

        private async Task ValidateMedicalHistoryHasNoActiveOccupancyAsync(int medicalHistoryId)
        {
            var existingMedicalHistoryOccupancy = await this.GetActiveOccupancyByMedicalHistoryIdAsync(medicalHistoryId);
            if (existingMedicalHistoryOccupancy != null)
            {
                throw new DataConflictException("Patient already has an active bed assignment");
            }
        }

        private async Task<BedOccupancy?> GetActiveOccupancyByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            return await _bedOccupancyRepository.GetByConditionAsync(
                occupancy => occupancy.MedicalHistoryId == medicalHistoryId
                    && occupancy.OccupancyStatus == OccupancyStatusEnum.Active);
        }

        private async Task<Bed> GetValidatedBedForOccupancyAsync(int bedId, int specialtyId)
        {
            var bed = await _bedRepository.GetByIdAsync(bedId, [nameof(Bed.Room)])
                ?? throw new DataNotFoundException(typeof(Bed), bedId);

            if (bed.Status != BedStatusEnum.Active)
            {
                throw new ValidationFailureException("Bed is not available for occupancy");
            }

            if (bed.Room == null)
            {
                throw new DataNotFoundException("Room was not found for the selected bed");
            }

            if (bed.Room.SpecialtyId != specialtyId)
            {
                throw new ValidationFailureException(
                    "Bed room specialty must match medical history specialty");
            }

            await this.ValidateBedHasNoActiveOccupancyAsync(bedId);

            return bed;
        }

        private async Task ValidateBedHasNoActiveOccupancyAsync(int bedId)
        {
            var existingBedOccupancy = await _bedOccupancyRepository.GetByConditionAsync(
                occupancy => occupancy.BedId == bedId
                    && occupancy.OccupancyStatus == OccupancyStatusEnum.Active);

            if (existingBedOccupancy != null)
            {
                throw new DataConflictException("Bed is already occupied");
            }
        }

        private async Task<Dictionary<int, (int BedCount, int AvailableBedCount)>> GetRoomBedCountDictionaryAsync(
            List<BedOccupancy> occupancies)
        {
            var roomIds = occupancies
                .Select(occupancy => occupancy.Bed?.RoomId)
                .OfType<int>()
                .Distinct()
                .ToList();

            if (roomIds.Count == 0)
            {
                return [];
            }

            var beds = await _bedRepository.GetAllAsync(bed => roomIds.Contains(bed.RoomId));

            return beds
                .GroupBy(bed => bed.RoomId)
                .ToDictionary(
                    group => group.Key,
                    group => (
                        BedCount: group.Count(),
                        AvailableBedCount: group.Count(bed => bed.Status == BedStatusEnum.Active)));
        }

        private void ApplyRoomBedCounts(
            GetBedOccupancyDTO dto,
            int? roomId,
            Dictionary<int, (int BedCount, int AvailableBedCount)> roomBedCountDict)
        {
            if (!roomId.HasValue || dto.Bed?.Room == null)
            {
                return;
            }

            if (roomBedCountDict.TryGetValue(roomId.Value, out var roomBedCounts))
            {
                dto.Bed.Room.BedCount = roomBedCounts.BedCount;
                dto.Bed.Room.AvailableBedCount = roomBedCounts.AvailableBedCount;
            }
        }

        private async Task<Dictionary<string, double>> GetBedCategoryPriceDictionaryAsync(
            List<BedOccupancy> occupancies)
        {
            var bedCategoryCodes = occupancies
                .Select(occupancy => occupancy.Bed?.BedCategory?.Code)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Cast<string>()
                .ToList();

            if (bedCategoryCodes.Count == 0)
            {
                return new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            }

            var medicalServiceContracts = await _messageBus.RequestAsync<
                GetListMedicalServiceDataByCodesEvent,
                GetListMedicalServiceDataContract>(
                    new GetListMedicalServiceDataByCodesEvent
                    {
                        Codes = bedCategoryCodes
                    });

            return medicalServiceContracts.Data
                .Where(contract => !string.IsNullOrWhiteSpace(contract.Code))
                .GroupBy(contract => contract.Code!, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    group => group.Key,
                    group => group.First().Price,
                    StringComparer.OrdinalIgnoreCase);
        }

        private void ApplyOccupancyPricing(
            GetBedOccupancyDTO dto,
            BedOccupancy occupancy,
            Dictionary<string, double> bedCategoryPriceDict)
        {
            dto.DurationInDays = this.CalculateOccupancyChargeQuantity(
                occupancy.FromDateTime,
                occupancy.ToDateTime);

            var bedCategoryCode = occupancy.Bed?.BedCategory?.Code;
            if (!string.IsNullOrWhiteSpace(bedCategoryCode)
                && bedCategoryPriceDict.TryGetValue(bedCategoryCode, out var unitPrice))
            {
                dto.UnitPrice = unitPrice;
            }

            dto.TotalAmount = Math.Round(
                dto.DurationInDays * dto.UnitPrice,
                2,
                MidpointRounding.AwayFromZero);
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

        private BedOccupancy CreateActiveBedOccupancy(int medicalHistoryId, int bedId)
        {
            return new BedOccupancy
            {
                MedicalHistoryId = medicalHistoryId,
                BedId = bedId,
                OccupancyStatus = OccupancyStatusEnum.Active
            };
        }

        private void UpdateOccupancyStatus(BedOccupancy occupancy, OccupancyStatusEnum occupancyStatus)
        {
            var occupancyStateMachine = new BedOccupancyStateMachine(occupancy);
            if (!occupancyStateMachine.CanFire(occupancyStatus))
            {
                throw new InvalidDataException(
                    $"Cannot change bed occupancy status from {occupancy.OccupancyStatus} to {occupancyStatus}");
            }

            try
            {
                occupancyStateMachine.Fire(occupancyStatus);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Failed to change bed occupancy status: {ex.Message}");
            }
        }

        private string? GetLatestTransferNote(List<BedOccupancy> occupancies)
        {
            return occupancies
                .Where(occupancy =>
                    occupancy.OccupancyStatus == OccupancyStatusEnum.Transferred
                    && !string.IsNullOrWhiteSpace(occupancy.TransferNote))
                .OrderByDescending(occupancy => occupancy.ToDateTime ?? occupancy.FromDateTime)
                .Select(occupancy => occupancy.TransferNote?.Trim())
                .FirstOrDefault();
        }

        private async Task<Dictionary<int, GetMedicalHistoryOccupancySummaryContract>>
            GetMedicalHistorySummaryDictionaryAsync(List<int> medicalHistoryIds)
        {
            if (medicalHistoryIds.Count == 0)
            {
                return [];
            }

            var contract = await _messageBus.RequestAsync<
                GetListMedicalHistoryOccupancySummaryByIdsEvent,
                GetListMedicalHistoryOccupancySummaryDataContract>(
                    new GetListMedicalHistoryOccupancySummaryByIdsEvent
                    {
                        Ids = medicalHistoryIds
                    });

            return contract.Data.ToDictionary(summary => summary.Id);
        }

        private async Task<Dictionary<int, BedOccupancy>> GetActiveOccupancyDictionaryByBedIdAsync(List<int> bedIds)
        {
            if (bedIds.Count == 0)
            {
                return [];
            }

            var activeOccupancies = await _bedOccupancyRepository.GetAllAsync(
                occupancy => bedIds.Contains(occupancy.BedId)
                    && occupancy.OccupancyStatus == OccupancyStatusEnum.Active);

            return activeOccupancies
                .GroupBy(occupancy => occupancy.BedId)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .OrderByDescending(occupancy => occupancy.FromDateTime)
                        .ThenByDescending(occupancy => occupancy.Id)
                        .First());
        }

        private GetBedOccupancySummaryDTO BuildOccupancySummaryDTO(
            BedOccupancy occupancy,
            Dictionary<int, GetMedicalHistoryOccupancySummaryContract> medicalHistorySummaryDict)
        {
            medicalHistorySummaryDict.TryGetValue(occupancy.MedicalHistoryId, out var medicalHistorySummary);

            return new GetBedOccupancySummaryDTO
            {
                BedOccupancyId = occupancy.Id,
                MedicalHistoryId = occupancy.MedicalHistoryId,
                MedicalHistoryNumber = medicalHistorySummary?.MedicalHistoryNumber,
                PatientId = medicalHistorySummary?.PatientId ?? 0,
                PatientName = medicalHistorySummary?.PatientName,
                DoctorId = medicalHistorySummary?.DoctorId,
                DoctorName = medicalHistorySummary?.DoctorName,
                MedicalHistoryStatus = medicalHistorySummary?.MedicalHistoryStatus,
                AdmissionDate = medicalHistorySummary?.AdmissionDate ?? occupancy.FromDateTime,
                DischargeDate = medicalHistorySummary?.DischargeDate,
                FromDateTime = occupancy.FromDateTime,
                ToDateTime = occupancy.ToDateTime,
                OccupancyStatus = occupancy.OccupancyStatus.ToString()
            };
        }

        private async Task RequestValueForRoomBoardAsync(List<GetBedOccupancyBoardRoomDTO> roomBoard)
        {
            var specialtyIds = roomBoard.Select(room => room.SpecialtyId).Distinct().ToList();

            if (specialtyIds.Count == 0)
            {
                return;
            }

            var specialtyContract = await _messageBus.RequestAsync<
                GetListSpecialtyDataByIdsEvent,
                GetListSpecialtyDataByIdsContract>(
                    new GetListSpecialtyDataByIdsEvent
                    {
                        SpecialtyIds = specialtyIds
                    });

            var specialtyDict = specialtyContract.Data.ToDictionary(specialty => specialty.Id);

            foreach (var room in roomBoard.Where(room => specialtyDict.ContainsKey(room.SpecialtyId)))
            {
                room.SpecialtyName = specialtyDict[room.SpecialtyId].Name;
            }
        }
        #endregion
    }
}
