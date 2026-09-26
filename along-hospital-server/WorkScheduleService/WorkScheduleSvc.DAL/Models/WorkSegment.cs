using SharedLibrary.Commons.EntityAbstractions;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.DAL.Models
{
    public class WorkSegment : AuditEntity
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public WorkStatusEnum WorkStatus { get; set; }

        public WorkStatusReasonEnum WorkStatusReason { get; set; }

        public int WorkScheduleAssignmentId { get; set; }
        public virtual WorkScheduleAssignment? WorkScheduleAssignment { get; set; }
    }
}
