using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.DAL.Models
{
    public class LeaveRequest : AuditEntity
    {
        public LeaveTypeEnum LeaveType { get; set; } = LeaveTypeEnum.Other;

        [MessageRequired]
        [DateValidator(NotAfter = nameof(ToDate))]
        public DateOnly FromDate { get; set; }

        [MessageRequired]
        [DateValidator(AllowPast = false)]
        public DateOnly ToDate { get; set; }

        [MessageMaxLength(255)]
        public string? Reason { get; set; }

        public RequestStatusEnum Status { get; set; } = RequestStatusEnum.Pending;

        public DateTime? DecidedAt { get; set; }

        public LeaveUnitEnum LeaveUnit { get; set; } = LeaveUnitEnum.Day;

        public int? ShiftId { get; set; }

        public int? DecidedBy { get; set; }
    }
}
