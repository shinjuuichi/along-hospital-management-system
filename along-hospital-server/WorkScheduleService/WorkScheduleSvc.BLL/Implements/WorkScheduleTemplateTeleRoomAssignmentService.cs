using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.TeleHealthEvents.TeleRoomEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForTeleRoom;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkScheduleTemplateTeleRoomAssignmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : IWorkScheduleTemplateTeleRoomAssignmentService
    {
        private readonly IGenericRepository<WorkScheduleTemplate> _templateRepository = unitOfWork.Repository<WorkScheduleTemplate>();
        private readonly IGenericRepository<Shift> _shiftRepository = unitOfWork.Repository<Shift>();
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetWorkScheduleTemplateAssignmentForStaffTeleRoomDTO>> GetTeleRoomAssignmentsAsync(
            int templateId,
            int? shiftId,
            int? teleRoomId,
            int? staffId)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffTeleRooms)]);

            var assignments = template.WorkScheduleTemplateAssignmentForStaffTeleRooms
                .Where(x =>
                    (!shiftId.HasValue || x.ShiftId == shiftId)
                    && (!teleRoomId.HasValue || x.TeleRoomId == teleRoomId)
                    && (!staffId.HasValue || x.StaffId == staffId))
                .ToList();

            var result = _mapper.Map<List<GetWorkScheduleTemplateAssignmentForStaffTeleRoomDTO>>(assignments);

            var staffLookup = new Dictionary<int, GetStaffDTO>();
            var teleRoomLookup = new Dictionary<int, GetTeleRoomDTO>();
            foreach (var dto in result)
            {
                if (!staffLookup.TryGetValue(dto.StaffId, out var staff))
                {
                    staff = await this.RequestValueForGetStaffDTOAsync(dto.StaffId);
                    staffLookup[dto.StaffId] = staff;
                }
                dto.Staff = staff;

                if (!teleRoomLookup.TryGetValue(dto.TeleRoomId, out var teleRoom))
                {
                    teleRoom = await this.RequestValueForGetTeleRoomDTOAsync(dto.TeleRoomId);
                    teleRoomLookup[dto.TeleRoomId] = teleRoom;
                }
                dto.TeleRoom = teleRoom;
            }

            return result;
        }

        public async Task CreateTeleRoomAssignmentsAsync(
            int templateId,
            CreateWorkScheduleTemplateAssignmentForStaffTeleRoomDTO createDTO)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffTeleRooms),
                 nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffRooms)]);

            if (createDTO.StaffIds.Count == 0)
            {
                throw new ValidationFailureException("StaffIds", "At least one staff is required.");
            }

            await this.ValidateShiftExistsAsync(createDTO.ShiftId);
            await this.EnsureTeleRoomExistsAsync(createDTO.TeleRoomId);

            var requestedStaffIds = createDTO.StaffIds.Distinct().ToList();
            var staffContract = await _messageBus.RequestAsync
                <GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>
                    (new GetListStaffDataByUserIdsEvent { UserIds = requestedStaffIds });
            var staffById = staffContract.Data.ToDictionary(s => s.UserId);
            var teleRoom = await this.RequestValueForGetTeleRoomDTOAsync(createDTO.TeleRoomId);

            var missingStaffIds = requestedStaffIds.Where(id => !staffById.ContainsKey(id)).ToList();
            if (missingStaffIds.Count > 0)
            {
                throw new DataNotFoundException($"Staff not found for IDs: [{string.Join(", ", missingStaffIds)}].");
            }

            foreach (var staffId in requestedStaffIds)
            {
                ValidateDoctorRole(staffById[staffId], staffId);
            }

            this.ValidateDoctorSpecialty(teleRoom.SpecialtyId, staffById, createDTO.TeleRoomId);

            var conflictingTeleAssignments = template.WorkScheduleTemplateAssignmentForStaffTeleRooms
                .Where(x =>
                    x.ShiftId == createDTO.ShiftId
                    && requestedStaffIds.Contains(x.StaffId)
                    && x.TeleRoomId != createDTO.TeleRoomId)
                .GroupBy(x => x.StaffId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.TeleRoomId).Distinct().ToList());

            if (conflictingTeleAssignments.Count > 0)
            {
                var conflictDetails = conflictingTeleAssignments
                    .Select(x => $"{x.Key} (tele-room: {string.Join(", ", x.Value)})")
                    .ToList();

                throw new ValidationFailureException(
                    "StaffIds",
                    $"Staff [{string.Join("; ", conflictDetails)}] already assigned to another tele-room in this shift. A staff can only be assigned to one room or tele-room in the same shift.");
            }

            var staffInRoomForShift = template.WorkScheduleTemplateAssignmentForStaffRooms
                .Where(x => x.ShiftId == createDTO.ShiftId)
                .Select(x => x.StaffId)
                .ToHashSet();

            var conflictingStaffIds = requestedStaffIds.Where(id => staffInRoomForShift.Contains(id)).ToList();
            if (conflictingStaffIds.Count > 0)
            {
                throw new ValidationFailureException("StaffIds",
                    $"Staff [{string.Join(", ", conflictingStaffIds)}] already assigned to a room in this shift. A staff cannot be assigned to both a room and a tele-room in the same shift");
            }

            var existingStaffIds = template.WorkScheduleTemplateAssignmentForStaffTeleRooms
                .Where(x => x.ShiftId == createDTO.ShiftId && x.TeleRoomId == createDTO.TeleRoomId)
                .Select(x => x.StaffId)
                .ToHashSet();

            var newStaffIds = requestedStaffIds
                .Where(staffId => !existingStaffIds.Contains(staffId))
                .ToList();

            if (existingStaffIds.Count + newStaffIds.Count > 1)
            {
                throw new ValidationFailureException("TeleRoomId", "Each tele-room can only be assigned to one doctor per shift.");
            }

            foreach (var staffId in newStaffIds)
            {
                template.WorkScheduleTemplateAssignmentForStaffTeleRooms.Add(new WorkScheduleTemplateAssignmentForStaffTeleRoom
                {
                    WorkScheduleTemplateId = templateId,
                    ShiftId = createDTO.ShiftId,
                    TeleRoomId = createDTO.TeleRoomId,
                    StaffId = staffId,
                });
            }

            if (newStaffIds.Count > 0)
            {
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task DeleteTeleRoomAssignmentAsync(int templateId, DeleteWorkScheduleTemplateAssignmentForStaffTeleRoomDTO deleteDTO)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffTeleRooms)]);

            var assignment = template.WorkScheduleTemplateAssignmentForStaffTeleRooms
                .FirstOrDefault(x =>
                    x.ShiftId == deleteDTO.ShiftId
                    && x.TeleRoomId == deleteDTO.TeleRoomId
                    && x.StaffId == deleteDTO.StaffId)
                ?? throw new DataNotFoundException("Assignment does not exist.");

            _unitOfWork.Repository<WorkScheduleTemplateAssignmentForStaffTeleRoom>().Remove(assignment);
            await _unitOfWork.SaveChangeAsync();
        }

        #region Helper methods
        private async Task ValidateShiftExistsAsync(int shiftId)
        {
            var count = await _shiftRepository.CountAsync(x => x.Id == shiftId);
            if (count == 0)
            {
                throw new DataNotFoundException(typeof(Shift), shiftId);
            }
        }

        private async Task EnsureTeleRoomExistsAsync(int teleRoomId)
        {
            await _messageBus.RequestAsync<GetTeleRoomByIdEvent, GetTeleRoomContract>(new GetTeleRoomByIdEvent
            {
                Id = teleRoomId,
            });
        }

        private static void ValidateDoctorRole(GetStaffDataByUserIdContract staffData, int staffId)
        {
            if (!string.Equals(staffData.Role, nameof(RoleEnum.Doctor)))
            {
                throw new ValidationFailureException("StaffId", $"Staff '{staffId}' must have role '{nameof(RoleEnum.Doctor)}'.");
            }
        }

        private void ValidateDoctorSpecialty(
            int teleRoomSpecialtyId,
            Dictionary<int, GetStaffDataByUserIdContract> staffById,
            int teleRoomId)
        {
            var invalidDoctorIds = staffById
                .Where(x => string.Equals(x.Value.Role, nameof(RoleEnum.Doctor), StringComparison.OrdinalIgnoreCase)
                    && x.Value.SpecialtyId != teleRoomSpecialtyId)
                .Select(x => x.Key)
                .ToList();

            if (invalidDoctorIds.Count > 0)
            {
                throw new ValidationFailureException(
                    "TeleRoomId",
                    $"Doctor [{string.Join(", ", invalidDoctorIds)}] must belong to the same specialty as tele-room '{teleRoomId}'.");
            }
        }

        private async Task<WorkScheduleTemplate> GetTemplateOrThrowAsync(int templateId, string[]? includes = null)
        {
            return await _templateRepository.GetByIdAsync(templateId, includes: includes)
                ?? throw new DataNotFoundException(typeof(WorkScheduleTemplate), templateId);
        }

        private async Task<GetStaffDTO> RequestValueForGetStaffDTOAsync(int staffId)
        {
            var staffContract = await _messageBus.RequestAsync
                <GetStaffDataByUserIdEvent, GetStaffDataByUserIdContract>
                    (new GetStaffDataByUserIdEvent { UserId = staffId });

            return _mapper.Map<GetStaffDTO>(staffContract);
        }

        private async Task<GetTeleRoomDTO> RequestValueForGetTeleRoomDTOAsync(int teleRoomId)
        {
            var teleRoomContract = await _messageBus.RequestAsync<GetTeleRoomByIdEvent, GetTeleRoomContract>(
                new GetTeleRoomByIdEvent { Id = teleRoomId });

            return _mapper.Map<GetTeleRoomDTO>(teleRoomContract);
        }
        #endregion
    }
}
