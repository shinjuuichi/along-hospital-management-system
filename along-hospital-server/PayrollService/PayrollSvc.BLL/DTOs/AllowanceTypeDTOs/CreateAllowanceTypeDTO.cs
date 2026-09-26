using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.AllowanceTypeDTOs
{
    public class CreateAllowanceTypeDTO : MapTo<AllowanceType>
    {
        #region Primary properties
        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public bool IsTaxable { get; set; }

        public string? InsuranceSubject { get; set; }
        #endregion
    }
}