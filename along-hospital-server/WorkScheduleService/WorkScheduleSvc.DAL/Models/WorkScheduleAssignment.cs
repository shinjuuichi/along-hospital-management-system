using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.DAL.Models
{
    [Index(nameof(WorkScheduleId), nameof(StaffId), IsUnique = true)]
    public class WorkScheduleAssignment : AuditEntity
    {
        public LocationTypeEnum LocationType { get; set; }

        public int StaffId { get; set; }

        public int LocationId { get; set; }

        public int WorkScheduleId { get; set; }
        public virtual WorkSchedule? WorkSchedule { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<WorkSegment> WorkSegments { get; set; } = [];
    }
}