using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDayShiftDTOs
{
    public class UpdateWorkScheduleTemplateDayShiftDTO
    {
        public DayOfWeekEnum DayOfWeek { get; set; }

        public int ShiftId { get; set; }
    }
}
