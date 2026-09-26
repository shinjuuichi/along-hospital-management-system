using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Interfaces.Validators
{
    public interface IWorkScheduleValidator
    {
        void ValidateDateRange(DateOnly fromDate, DateOnly toDate);
        void ValidateGenerateMode(CreateWorkScheduleDTO dto);
        Task ValidateShiftIdAsync(int shiftId);
        Task ValidateTemplateActiveAsync(int templateId);
        void ValidateWorkScheduleRange(GetWorkScheduleRangeDTO rangeDTO);
        void ValidateWorkScheduleIds(List<int> workScheduleIds);
        void ValidateWorkSchedulesFound(List<WorkSchedule> workSchedules);
        Task<bool> IsHolidayAsync(DateOnly date);
        Task ValidateWorkScheduleAsync(DateOnly workDate, int shiftId, int? excludeId = null);
        void ValidateWorkScheduleMutableStatus(WorkSchedule workSchedule);
        void ValidateStatusTransition(WorkSchedule workSchedule, WorkScheduleStatusEnum newStatus);
    }
}
