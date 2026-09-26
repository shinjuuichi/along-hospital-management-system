using PayrollSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace PayrollSvc.DAL.Models
{
    public class DeductionType : AuditEntity
    {
        #region Primary properties
        [MessageRequired, MessageMaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        [NumberHigherThan(0)]
        public double Price { get; set; }

        public bool IsTaxable { get; set; }

        public bool IsSystemGenerated { get; set; }

        public bool IsPercentage { get; set; }

        [MessageRequired]
        public InsuranceSubjectEnum InsuranceSubject { get; set; }

        public DeductionQuantitySourceEnum DeductionQuantitySourceEnum { get; set; } = DeductionQuantitySourceEnum.None;
        #endregion

        #region Foreign keys
        public virtual ICollection<Deduction> Deductions { get; set; } = [];
        #endregion
    }
}