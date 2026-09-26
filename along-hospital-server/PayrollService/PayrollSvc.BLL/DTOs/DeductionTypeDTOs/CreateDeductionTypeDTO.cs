using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.DeductionTypeDTOs
{
    public class CreateDeductionTypeDTO : MapTo<DeductionType>
    {
        #region Primary properties
        public string? Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public bool IsTaxable { get; set; }
        #endregion
    }
}