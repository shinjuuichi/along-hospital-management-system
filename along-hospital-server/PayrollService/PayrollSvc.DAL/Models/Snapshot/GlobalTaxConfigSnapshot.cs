using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models.Snapshot
{
    public class GlobalTaxConfigSnapshot : Entity
    {
        public double ReferenceBaseSalary { get; set; }

        public double PersonalDeductionAmount { get; set; }

        public double DependentDeductionAmount { get; set; }

        public double SocialInsuranceRate { get; set; }

        public double HealthInsuranceRate { get; set; }

        public double UnemploymentInsuranceRate { get; set; }
    }
}