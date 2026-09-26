using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace PayrollSvc.DAL.Models
{
    public class Deduction : BaseEntity
    {
        #region Primary properties
        [NumberHigherThan(0)]
        public double UnitPrice { get; set; }

        [NumberHigherThanOrEqualTo(1)]
        public int Quantity { get; set; }

        public bool IsTaxable { get; set; }

        [MessageMaxLength(100)]
        public string? Note { get; set; }
        #endregion

        #region Foreign keys
        public int DeductionTypeId { get; set; }

        public string DeductionTypeName { get; set; } = string.Empty;

        public int PayrollId { get; set; }

        public int? PayrollPolicyId { get; set; }

        public virtual Payroll? Payroll { get; set; }
        public virtual DeductionType? DeductionType { get; set; }
        public virtual PayrollPolicy? PayrollPolicy { get; set; }
        #endregion
    }
}