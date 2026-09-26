using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.DAL.Models
{
    public class SalaryAdvance : AuditEntity
    {
        [NumberHigherThan(0)]
        public double Amount { get; set; }

        [MessageRequired, MessageMaxLength(255)]
        public string Reason { get; set; } = string.Empty;

        public SalaryAdvanceStatusEnum Status { get; set; } = SalaryAdvanceStatusEnum.Pending;

        public DateTime? DecidedAt { get; set; }

        public int? DecidedBy { get; set; }

        public int? PayrollId { get; set; }
    }
}