using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.InPatientResourceEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForStaffRoom;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkScheduleTemplateRoomAssignmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : IWorkScheduleTemplateRoomAssignmentService
    {
        private readonly IGenericRepository<WorkScheduleTemplate> _templateRepository = unitOfWork.Repository<WorkScheduleTemplate>();
        private readonly IGenericRepository<Shift> _shiftRepository = unitOfWork.Repository<Shift>();
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetWorkScheduleTemplateAssignmentForStaffRoomDTO>> GetRoomAssignmentsAsync(
            int templateId,
            int? shiftId,
            int? roomId,
            int? staffId)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffRooms)]);

            var assignments = template.WorkScheduleTemplateAssignmentForStaffRooms
                .Where(x =>
                    (!shiftId.HasValue || x.ShiftId == shiftId)
                    && (!roomId.HasValue || x.RoomId == roomId)
                    && (!staffId.HasValue || x.StaffId == staffId))
                .ToList();

            var result = _mapper.Map<List<GetWorkScheduleTemplateAssignmentForStaffRoomDTO>>(assignments);

            var staffLookup = new Dictionary<int, GetStaffDTO>();
            var roomLookup = new Dictionary<int, GetRoomDTO>();
            foreach (var dto in result)
            {
                if (!staffLookup.TryGetValue(dto.StaffId, out var staff))
                {
                    staff = await this.RequestValueForGetStaffDTOAsync(dto.StaffId);
                    staffLookup[dto.StaffId] = staff;
                }
                dto.Staff = staff;

                if (!roomLookup.TryGetValue(dto.RoomId, out var room))
                {
                    room = await this.RequestValueForGetRoomDTOAsync(dto.RoomId);
                    roomLookup[dto.RoomId] = room;
                }
                dto.Room = room;
            }

            return result;
        }

        public async Task CreateRoomAssignmentsAsync(
            int templateId,
            CreateWorkScheduleTemplateAssignmentForStaffRoomDTO createDTO)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffRooms),
                 nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffTeleRooms)]);

            if (createDTO.StaffIds.Count == 0)
            {
                throw new ValidationFailureException("StaffIds", "At least one staff is required.");
            }

            await this.ValidateShiftExistsAsync(createDTO.ShiftId);
            var room = await this.GetRoomOrThrowAsync(createDTO.RoomId);

            var requestedStaffIds = createDTO.StaffIds.Distinct().ToList();
            var staffContract = await _messageBus.RequestAsync
                <GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>
                    (new GetListStaffDataByUserIdsEvent { UserIds = requestedStaffIds });
            var staffById = staffContract.Data.ToDictionary(s => s.UserId);
            this.ValidateRoomAssignmentRoles(room, staffById, createDTO.RoomId);
            this.ValidateDoctorSpecialty(room.SpecialtyId, staffById, createDTO.RoomId, nameof(WorkScheduleTemplateAssignmentForStaffRoom.RoomId));

            var conflictingRoomAssignments = template.WorkScheduleTemplateAssignmentForStaffRooms
                .Where(x =>
                    x.ShiftId == createDTO.ShiftId
                    && requestedStaffIds.Contains(x.StaffId)
                    && x.RoomId != createDTO.RoomId)
                .GroupBy(x => x.StaffId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.RoomId).Distinct().ToList());

            if (conflictingRoomAssignments.Count > 0)
            {
                var conflictDetails = conflictingRoomAssignments
                    .Select(x => $"{x.Key} (room: {string.Join(", ", x.Value)})")
                    .ToList();

                throw new ValidationFailureException(
                    "StaffIds",
                    $"Staff [{string.Join("; ", conflictDetails)}] already assigned to another room in this shift. A staff can only be assigned to one room or tele-room in the same shift.");
            }

            var staffInTeleRoomForShift = template.WorkScheduleTemplateAssignmentForStaffTeleRooms
                .Where(x => x.ShiftId == createDTO.ShiftId)
                .Select(x => x.StaffId)
                .ToHashSet();

            var conflictingStaffIds = requestedStaffIds.Where(id => staffInTeleRoomForShift.Contains(id)).ToList();
            if (conflictingStaffIds.Count > 0)
            {
                throw new ValidationFailureException("StaffIds",
                    $"Staff [{string.Join(", ", conflictingStaffIds)}] already assigned to a tele-room in this shift. A staff cannot be assigned to both a room and a tele-room in the same shift.");
            }

            var existingStaffIds = template.WorkScheduleTemplateAssignmentForStaffRooms
                .Where(x => x.ShiftId == createDTO.ShiftId && x.RoomId == createDTO.RoomId)
                .Select(x => x.StaffId)
                .ToHashSet();

            var newStaffIds = requestedStaffIds
                .Where(staffId => !existingStaffIds.Contains(staffId))
                .ToList();

            foreach (var staffId in newStaffIds)
            {
                template.WorkScheduleTemplateAssignmentForStaffRooms.Add(new WorkScheduleTemplateAssignmentForStaffRoom
                {
                    WorkScheduleTemplateId = templateId,
                    ShiftId = createDTO.ShiftId,
                    RoomId = createDTO.RoomId,
                    StaffId = staffId,
                });
            }

            if (newStaffIds.Count > 0)
            {
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task DeleteRoomAssignmentAsync(int templateId, DeleteWorkScheduleTemplateAssignmentForStaffRoomDTO deleteDTO)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffRooms)]);

            var assignment = template.WorkScheduleTemplateAssignmentForStaffRooms
                .FirstOrDefault(x =>
                    x.ShiftId == deleteDTO.ShiftId
                    && x.RoomId == deleteDTO.RoomId
                    && x.StaffId == deleteDTO.StaffId)
                ?? throw new DataNotFoundException("Assignment does not exist.");

            _unitOfWork.Repository<WorkScheduleTemplateAssignmentForStaffRoom>().Remove(assignment);
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

        private async Task<GetRoomContract> GetRoomOrThrowAsync(int roomId)
        {
            return await _messageBus.RequestAsync<GetRoomByIdEvent, GetRoomContract>(new GetRoomByIdEvent
            {
                Id = roomId,
            });
        }

        private void ValidateRoomAssignmentRoles(
            GetRoomContract room,
            Dictionary<int, GetStaffDataByUserIdContract> staffById,
            int roomId)
        {
            var allowedRoles = (room.Roles ?? [])
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (allowedRoles.Count == 0)
            {
                throw new ValidationFailureException("RoomId", $"Room '{roomId}' does not have any allowed role mapping.");
            }

            var allowedRoleSet = allowedRoles.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var invalidStaff = staffById
                .Where(x => string.IsNullOrWhiteSpace(x.Value.Role) || !allowedRoleSet.Contains(x.Value.Role!))
                .Select(x => $"{x.Key} ({x.Value.Role ?? "Unknown"})")
                .ToList();

            if (invalidStaff.Count > 0)
            {
                throw new ValidationFailureException(
                    "StaffIds",
                    $"Staff role is not allowed for room '{roomId}'. Allowed roles: {string.Join(", ", allowedRoles)}. Invalid staff: {string.Join(", ", invalidStaff)}.");
            }
        }

        private void ValidateDoctorSpecialty(
            int roomSpecialtyId,
            Dictionary<int, GetStaffDataByUserIdContract> staffById,
            int roomId,
            string propertyName)
        {
            var invalidDoctorIds = staffById
                .Where(x => string.Equals(x.Value.Role, nameof(RoleEnum.Doctor), StringComparison.OrdinalIgnoreCase)
                    && x.Value.SpecialtyId != roomSpecialtyId)
                .Select(x => x.Key)
                .ToList();

            if (invalidDoctorIds.Count > 0)
            {
                throw new ValidationFailureException(
                    propertyName,
                    $"Doctor [{string.Join(", ", invalidDoctorIds)}] must belong to the same specialty as room '{roomId}'.");
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

        private async Task<GetRoomDTO> RequestValueForGetRoomDTOAsync(int roomId)
        {
            var roomContract = await _messageBus.RequestAsync<GetRoomByIdEvent, GetRoomContract>(
                new GetRoomByIdEvent { Id = roomId });

            return _mapper.Map<GetRoomDTO>(roomContract);
        }
        #endregion
    }
}
