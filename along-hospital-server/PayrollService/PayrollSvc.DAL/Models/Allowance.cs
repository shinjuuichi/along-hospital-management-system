using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace PayrollSvc.DAL.Models
{
    public class Allowance : BaseEntity
    {
        #region Primary properties
        [NumberHigherThan(0)]
        public double UnitPrice { get; set; }

        [NumberHigherThanOrEqualTo(1)]
        public int Quantity { get; set; }

        [MessageMaxLength(100)]
        public string? Note { get; set; }

        public bool IsTaxable { get; set; }
        #endregion

        #region Foreign keys
        public int PayrollId { get; set; }

        public int? PayrollPolicyId { get; set; }

        public int AllowanceTypeId { get; set; }

        public string AllowanceTypeName { get; set; } = string.Empty;

        public virtual Payroll? Payroll { get; set; }
        public virtual AllowanceType? AllowanceType { get; set; }
        public virtual PayrollPolicy? PayrollPolicy { get; set; }
        #endregion
    }
}