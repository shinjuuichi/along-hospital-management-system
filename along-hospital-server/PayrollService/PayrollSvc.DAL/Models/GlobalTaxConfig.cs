using System.ComponentModel.DataAnnotations.Schema;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace PayrollSvc.DAL.Models
{
    public class GlobalTaxConfig : AuditEntity
    {
        #region Primary properties
        [NumberHigherThan(0)]
        public double PersonalDeductionAmount { get; set; }

        [NumberHigherThan(0)]
        public double DependentDeductionAmount { get; set; }

        [NumberHigherThan(0)]
        public double ReferenceBaseSalary { get; set; }

        [MessageRange(0, 1), Column(TypeName = "numeric(18,4)")]
        public double SocialInsuranceRate { get; set; }

        [MessageRange(0, 1), Column(TypeName = "numeric(18,4)")]
        public double HealthInsuranceRate { get; set; }

        [MessageRange(0, 1), Column(TypeName = "numeric(18,4)")]
        public double UnemploymentInsuranceRate { get; set; }
        #endregion
    }
}