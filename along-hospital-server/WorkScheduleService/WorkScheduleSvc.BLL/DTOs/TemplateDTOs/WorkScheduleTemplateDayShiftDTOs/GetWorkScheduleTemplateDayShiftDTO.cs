using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDayShiftDTOs
{
    public class GetWorkScheduleTemplateDayShiftDTO : MapFrom<WorkScheduleTemplateDayShift>
    {
        public int Id { get; set; }

        public DayOfWeekEnum DayOfWeek { get; set; }

        public int WorkScheduleTemplateId { get; set; }

        public int ShiftId { get; set; }
    }
}
