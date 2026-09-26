using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Interfaces.Querys
{
    public interface IWorkScheduleQueryService
    {
        Task<GetWorkScheduleDTO> BuildWorkScheduleDTOAsync(WorkSchedule workSchedule);
        Task<List<GetWorkScheduleDTO>> BuildWorkScheduleDTOsAsync(List<WorkSchedule> workSchedules);
        Task<List<GetWorkScheduleDTO>> GetWorkSchedulesForDateRangeAsync(GetWorkScheduleRangeDTO rangeDTO);
        Task<List<GetWorkScheduleDTO>> GetWorkSchedulesForCurrentStaffAsync(GetWorkScheduleRangeDTO rangeDTO);
        Task<List<(DayOfWeekEnum DayOfWeek, int ShiftId)>> GetTemplateDayShiftsAsync(int templateId);
        Task<List<WorkScheduleAssignment>> GetWorkScheduleAssignmentsAsync(int workScheduleId);
        Task<List<WorkScheduleAssignment>> BuildTemplateAssignmentsAsync(int workScheduleId, int? templateId, int shiftId);
        Task<int?> GetTodayWorkingDoctorIdByRoomIdAsync(int roomId);
        Task ValidateStaffActionByRoomIdAsync(int staffId, int roomId);
        Task<List<RoomDoctorInfoDTO>> GetListTodayWorkingByQueueManagementRoleAsync();
        Task<List<GetStaffDTO>> GetAllCurrentWorkingDoctorsBySpecialtyAsync(int? specialtyId);
    }
}
