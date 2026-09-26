using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Gateways;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.QueryServices
{
    public class WorkScheduleQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        WorkScheduleAssignmentDetailsQueryHelper assignmentDetailsQueryHelper,
        ICurrentUserService currentUserService,
        IWorkScheduleAssignmentGateway workScheduleAssignmentGateway)
        : IWorkScheduleQueryService
    {
        private readonly IGenericRepository<WorkScheduleTemplateDayShift> _templateDayShiftRepository = unitOfWork.Repository<WorkScheduleTemplateDayShift>();
        private readonly IGenericRepository<WorkSchedule> _workScheduleRepository = unitOfWork.Repository<WorkSchedule>();
        private readonly IGenericRepository<WorkScheduleAssignment> _workScheduleAssignmentRepository = unitOfWork.Repository<WorkScheduleAssignment>();
        private readonly IGenericRepository<WorkScheduleTemplateAssignmentForStaffRoom> _templateRoomAssignmentRepository = unitOfWork.Repository<WorkScheduleTemplateAssignmentForStaffRoom>();
        private readonly IGenericRepository<WorkScheduleTemplateAssignmentForStaffTeleRoom> _templateTeleRoomAssignmentRepository = unitOfWork.Repository<WorkScheduleTemplateAssignmentForStaffTeleRoom>();
        private readonly IGenericRepository<WorkSegment> _workSegmentRepository = unitOfWork.Repository<WorkSegment>();

        private readonly IMapper _mapper = mapper;
        private readonly WorkScheduleAssignmentDetailsQueryHelper _assignmentDetailsQueryHelper = assignmentDetailsQueryHelper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IWorkScheduleAssignmentGateway _workScheduleAssignmentGateway = workScheduleAssignmentGateway;

        #region Primary Methods
        public async Task<GetWorkScheduleDTO> BuildWorkScheduleDTOAsync(WorkSchedule workSchedule)
        {
            var workScheduleDTOs = await BuildWorkScheduleDTOsAsync([workSchedule]);
            return workScheduleDTOs[0];
        }

        public async Task<List<GetWorkScheduleDTO>> BuildWorkScheduleDTOsAsync(List<WorkSchedule> workSchedules)
        {
            if (workSchedules.Count == 0)
            {
                return [];
            }

            var workScheduleDTOs = _mapper.Map<List<GetWorkScheduleDTO>>(workSchedules);
            var workScheduleIds = workScheduleDTOs
                .Select(x => x.Id)
                .Distinct()
                .ToList();
            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x => workScheduleIds.Contains(x.WorkScheduleId));
            var assignmentDTOs = await BuildAssignmentDTOsAsync(assignments);
            var assignmentLookup = assignmentDTOs
                .GroupBy(x => x.WorkScheduleId)
                .ToDictionary(x => x.Key, x => x.ToList());
            var assignmentIds = assignmentDTOs
                .Select(x => x.Id)
                .Distinct()
                .ToList();
            var workSegmentLookup = await BuildWorkSegmentLookupAsync(assignmentIds);

            foreach (var workScheduleDTO in workScheduleDTOs)
            {
                if (!assignmentLookup.TryGetValue(workScheduleDTO.Id, out var scheduleAssignmentDTOs))
                {
                    continue;
                }

                foreach (var assignmentDTO in scheduleAssignmentDTOs)
                {
                    if (workSegmentLookup.TryGetValue(assignmentDTO.Id, out var assignmentSegmentDTOs))
                    {
                        assignmentDTO.WorkSegments = assignmentSegmentDTOs;
                    }
                }

                workScheduleDTO.WorkScheduleAssignments = scheduleAssignmentDTOs;
            }

            return workScheduleDTOs;
        }

        public async Task<List<GetWorkScheduleDTO>> GetWorkSchedulesForCurrentStaffAsync(GetWorkScheduleRangeDTO rangeDTO)
        {
            var staffId = _currentUserService.UserId;
            var workSchedules = await _workScheduleRepository.GetAllAsync(x =>
                x.WorkScheduleAssignments.Any(a => a.StaffId == staffId)
                && x.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && (!rangeDTO.FromDate.HasValue || x.WorkDate >= rangeDTO.FromDate.Value)
                && (!rangeDTO.ToDate.HasValue || x.WorkDate <= rangeDTO.ToDate.Value),
                [nameof(WorkSchedule.Shift)]);

            return await BuildWorkScheduleDTOsForStaffAsync(workSchedules, staffId);
        }

        public async Task<List<GetWorkScheduleDTO>> GetWorkSchedulesForDateRangeAsync(GetWorkScheduleRangeDTO rangeDTO)
        {
            var workSchedules = await _workScheduleRepository.GetAllAsync(x =>
            (!rangeDTO.FromDate.HasValue || x.WorkDate >= rangeDTO.FromDate.Value)
            && (!rangeDTO.ToDate.HasValue || x.WorkDate <= rangeDTO.ToDate.Value),
            [nameof(WorkSchedule.Shift)]);

            return await BuildWorkScheduleDTOsAsync(workSchedules);
        }

        public async Task<List<(DayOfWeekEnum DayOfWeek, int ShiftId)>> GetTemplateDayShiftsAsync(int templateId)
        {
            var templateDayShifts = await _templateDayShiftRepository.GetAllAsync(x => x.WorkScheduleTemplateId == templateId);
            return templateDayShifts
                .Select(x => (x.DayOfWeek, x.ShiftId))
                .ToList();
        }

        public async Task<List<WorkScheduleAssignment>> GetWorkScheduleAssignmentsAsync(int workScheduleId)
        {
            return await _workScheduleAssignmentRepository.GetAllAsync(x => x.WorkScheduleId == workScheduleId);
        }

        public async Task<List<WorkScheduleAssignment>> BuildTemplateAssignmentsAsync(int workScheduleId, int? templateId, int shiftId)
        {
            if (!templateId.HasValue)
            {
                return [];
            }

            var roomAssignments = await _templateRoomAssignmentRepository.GetAllAsync(x =>
                x.WorkScheduleTemplateId == templateId.Value && x.ShiftId == shiftId);
            var teleRoomAssignments = await _templateTeleRoomAssignmentRepository.GetAllAsync(x =>
                x.WorkScheduleTemplateId == templateId.Value && x.ShiftId == shiftId);

            return roomAssignments.Select(x => new WorkScheduleAssignment
            {
                WorkScheduleId = workScheduleId,
                StaffId = x.StaffId,
                LocationId = x.RoomId,
                LocationType = LocationTypeEnum.Room
            }).Concat(teleRoomAssignments.Select(x => new WorkScheduleAssignment
            {
                WorkScheduleId = workScheduleId,
                StaffId = x.StaffId,
                LocationId = x.TeleRoomId,
                LocationType = LocationTypeEnum.TeleRoom
            })).ToList();
        }

        public async Task ValidateStaffActionByRoomIdAsync(int staffId, int roomId)
        {
            var nowLocal = DateTime.UtcNow.ConvertTimeToTimeZone();

            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var hasValidAssignment = await _workScheduleAssignmentRepository.AnyAsync(x =>
                x.StaffId == staffId
                && x.LocationType == LocationTypeEnum.Room
                && x.LocationId == roomId
                && x.WorkSchedule!.WorkDate == today
                && x.WorkSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && x.WorkSchedule.Shift!.StartTime <= nowTime
                && x.WorkSchedule.Shift.EndTime >= nowTime);

            if (!hasValidAssignment)
            {
                throw new UnauthorizedAccessException("Staff does not have a valid assignment for the specified room and time.");
            }
        }

        public async Task<int?> GetTodayWorkingDoctorIdByRoomIdAsync(int roomId)
        {
            var nowLocal = DateTime.UtcNow.ConvertTimeToTimeZone();

            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var assignment = await _workScheduleAssignmentRepository.GetByConditionAsync(x =>
                x.LocationType == LocationTypeEnum.Room
                && x.LocationId == roomId
                && x.WorkSchedule!.WorkDate == today
                && x.WorkSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && x.WorkSchedule.Shift!.StartTime <= nowTime
                && x.WorkSchedule.Shift.EndTime >= nowTime);

            return assignment?.StaffId;
        }

        public async Task<List<RoomDoctorInfoDTO>> GetListTodayWorkingByQueueManagementRoleAsync()
        {
            var staffDTOs = await _workScheduleAssignmentGateway.GetListStaffDataByRoleAsync();

            var staffRoleMap = staffDTOs.ToDictionary(x => x.UserId, x => x.Role);
            var staffIds = staffRoleMap.Keys.ToList();

            if (staffIds.Count == 0)
            {
                return [];
            }

            var nowLocal = DateTime.UtcNow.ConvertTimeToTimeZone();

            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x =>
                x.LocationType == LocationTypeEnum.Room
                && x.WorkSchedule!.WorkDate == today
                && x.WorkSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && x.WorkSchedule.Shift!.StartTime <= nowTime
                && x.WorkSchedule.Shift.EndTime >= nowTime
                && staffIds.Contains(x.StaffId));

            var roomDoctorInfoList = assignments.Select(x =>
            {
                bool isDoctor = staffRoleMap.TryGetValue(x.StaffId, out var role) && role == nameof(RoleEnum.Doctor);

                return new RoomDoctorInfoDTO
                {
                    RoomId = x.LocationId,
                    DoctorId = isDoctor ? x.StaffId : null,
                };
            }).ToList();

            return roomDoctorInfoList;
        }

        public async Task<List<GetStaffDTO>> GetAllCurrentWorkingDoctorsBySpecialtyAsync(int? specialtyId)
        {
            if (!specialtyId.HasValue)
            {
                return [];
            }

            var doctorStaffDTOs = await _workScheduleAssignmentGateway.GetDoctorStaffDataAsync(specialtyId);

            if (doctorStaffDTOs.Count == 0)
            {
                return [];
            }

            var doctorIds = doctorStaffDTOs
                .Select(x => x.UserId)
                .Distinct()
                .ToList();

            var nowLocal = DateTime.UtcNow.ConvertTimeToTimeZone();
            var today = DateOnly.FromDateTime(nowLocal);
            var nowTime = TimeOnly.FromDateTime(nowLocal);

            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x =>
                doctorIds.Contains(x.StaffId)
                && x.WorkSchedule!.WorkDate == today
                && x.WorkSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && x.WorkSchedule.Shift!.StartTime <= nowTime
                && x.WorkSchedule.Shift.EndTime >= nowTime);

            if (assignments.Count == 0)
            {
                return [];
            }

            var workingDoctorIds = assignments
                .Select(x => x.StaffId)
                .Distinct()
                .ToHashSet();

            return doctorStaffDTOs
                .Where(x => workingDoctorIds.Contains(x.UserId))
                .ToList();
        }
        #endregion

        #region Helper Methods
        private async Task<List<GetWorkScheduleAssignmentDTO>> BuildAssignmentDTOsAsync(List<WorkScheduleAssignment> assignments)
        {
            var assignmentDTOs = _mapper.Map<List<GetWorkScheduleAssignmentDTO>>(assignments);
            await _assignmentDetailsQueryHelper.PopulateAssignmentDetailsAsync(assignmentDTOs);
            return assignmentDTOs;
        }

        private async Task<List<GetWorkScheduleDTO>> BuildWorkScheduleDTOsForStaffAsync(List<WorkSchedule> workSchedules, int staffId)
        {
            if (workSchedules.Count == 0)
            {
                return [];
            }

            var workScheduleDTOs = _mapper.Map<List<GetWorkScheduleDTO>>(workSchedules);
            var workScheduleIds = workScheduleDTOs
                .Select(x => x.Id)
                .Distinct()
                .ToList();
            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x =>
                workScheduleIds.Contains(x.WorkScheduleId) && x.StaffId == staffId);
            var assignmentDTOs = await BuildAssignmentDTOsAsync(assignments);
            var assignmentLookup = assignmentDTOs
                .GroupBy(x => x.WorkScheduleId)
                .ToDictionary(x => x.Key, x => x.ToList());
            var assignmentIds = assignmentDTOs
                .Select(x => x.Id)
                .Distinct()
                .ToList();
            var workSegmentLookup = await BuildWorkSegmentLookupAsync(assignmentIds);

            foreach (var workScheduleDTO in workScheduleDTOs)
            {
                if (!assignmentLookup.TryGetValue(workScheduleDTO.Id, out var scheduleAssignmentDTOs))
                {
                    workScheduleDTO.WorkScheduleAssignments = [];
                    continue;
                }

                foreach (var assignmentDTO in scheduleAssignmentDTOs)
                {
                    if (workSegmentLookup.TryGetValue(assignmentDTO.Id, out var assignmentSegmentDTOs))
                    {
                        assignmentDTO.WorkSegments = assignmentSegmentDTOs;
                    }
                }

                workScheduleDTO.WorkScheduleAssignments = scheduleAssignmentDTOs;
            }

            return workScheduleDTOs;
        }

        private async Task<Dictionary<int, List<GetWorkSegmentDTO>>> BuildWorkSegmentLookupAsync(List<int> assignmentIds)
        {
            if (assignmentIds.Count == 0)
            {
                return [];
            }

            var workSegments = await _workSegmentRepository.GetAllAsync(x => assignmentIds.Contains(x.WorkScheduleAssignmentId));
            var workSegmentDTOs = _mapper.Map<List<GetWorkSegmentDTO>>(workSegments);
            return workSegmentDTOs
                .GroupBy(x => x.WorkScheduleAssignmentId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.StartTime).ToList());
        }
        #endregion
    }
}
