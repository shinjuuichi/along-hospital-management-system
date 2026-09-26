using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.AllowanceTypeDTOs
{
    public class GetAllowanceTypeDTO : MapFrom<AllowanceType>
    {
        #region Primary properties
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public bool IsTaxable { get; set; }

        public bool IsSystemGenerated { get; set; }

        public bool IsPercentage { get; set; }

        public string? AllowanceQuantitySourceEnum { get; set; }

        public string? InsuranceSubject { get; set; }
        #endregion
    }
}