using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;

namespace WorkScheduleSvc.DAL.Models
{
    public class Shift : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MessageRequired]
        [TimeValidator(NotAfter = nameof(EndTime))]
        public TimeOnly StartTime { get; set; }

        [MessageRequired]
        public TimeOnly EndTime { get; set; }

        public bool IsOvertime { get; set; }

        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; } = [];
        public virtual ICollection<WorkScheduleTemplateDayShift> WorkScheduleTemplateDayShifts { get; set; } = [];
    }
}