using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.SnapshotDTOs.GlobalTaxConfigSnapshotDTOs
{
    public class GetGlobalTaxConfigSnapshotDTO : MapFrom<GlobalTaxConfigSnapshot>
    {
        public double ReferenceBaseSalary { get; set; }

        public double PersonalDeductionAmount { get; set; }

        public double DependentDeductionAmount { get; set; }

        public double SocialInsuranceRate { get; set; }

        public double HealthInsuranceRate { get; set; }

        public double UnemploymentInsuranceRate { get; set; }
    }
}