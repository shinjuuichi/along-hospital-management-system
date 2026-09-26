using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.DAL.Models
{
    [Index(nameof(WorkScheduleTemplateId), nameof(DayOfWeek), nameof(ShiftId), IsUnique = true)]
    public class WorkScheduleTemplateDayShift : AuditEntity
    {
        [MessageRequired]
        public DayOfWeekEnum DayOfWeek { get; set; }

        [MessageRequired]
        public int WorkScheduleTemplateId { get; set; }

        [MessageRequired]
        public int ShiftId { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public virtual WorkScheduleTemplate? WorkScheduleTemplate { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public virtual Shift? Shift { get; set; }
    }
}
