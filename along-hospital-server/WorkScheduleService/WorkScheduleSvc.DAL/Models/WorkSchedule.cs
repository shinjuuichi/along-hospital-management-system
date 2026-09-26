using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.DAL.Models
{
    [Index(nameof(WorkDate), nameof(ShiftId), IsUnique = true)]
    public class WorkSchedule : AuditEntity
    {
        [DateValidator(AllowPast = false)]
        public DateOnly WorkDate { get; set; }

        public int ShiftId { get; set; }
        public virtual Shift? Shift { get; set; }

        public int? WorkScheduleTemplateId { get; set; }
        public virtual WorkScheduleTemplate? WorkScheduleTemplate { get; set; }

        public WorkScheduleStatusEnum WorkScheduleStatus { get; set; } = WorkScheduleStatusEnum.Draft;

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<WorkScheduleAssignment> WorkScheduleAssignments { get; set; } = [];
    }
}
