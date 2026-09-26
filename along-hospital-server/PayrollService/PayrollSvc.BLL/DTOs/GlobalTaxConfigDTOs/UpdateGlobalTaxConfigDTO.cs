using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.GlobalTaxConfigDTOs
{
    public class UpdateGlobalTaxConfigDTO : MapTo<GlobalTaxConfig>
    {
        #region Primary properties
        public double PersonalDeductionAmount { get; set; }

        public double DependentDeductionAmount { get; set; }

        public double ReferenceBaseSalary { get; set; }

        public double SocialInsuranceRate { get; set; }

        public double HealthInsuranceRate { get; set; }

        public double UnemploymentInsuranceRate { get; set; }
        #endregion
    }
}