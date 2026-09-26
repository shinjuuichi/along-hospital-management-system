using PayrollSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace PayrollSvc.DAL.Models
{
    public class PayrollPolicy : AuditEntity
    {
        #region Primary properties
        [MessageRequired, MessageMaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        [DateValidator(NotBefore = nameof(StartDate), AllowPast = false)]
        public DateOnly EndDate { get; set; }

        public PayrollPolicyStatus PayrollPolicyStatus { get; set; } = PayrollPolicyStatus.Active;
        #endregion

        #region Foreign keys
        public int? AllowanceTypeId { get; set; }
        public int? DeductionTypeId { get; set; }

        public virtual AllowanceType? AllowanceType { get; set; }
        public virtual DeductionType? DeductionType { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<PayrollPolicyStaff> PayrollPolicyStaffs { get; set; } = [];
        #endregion
    }
}