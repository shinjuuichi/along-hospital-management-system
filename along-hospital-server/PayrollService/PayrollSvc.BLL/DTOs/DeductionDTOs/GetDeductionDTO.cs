using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.DeductionDTOs
{
    public class GetDeductionDTO : MapFrom<Deduction>
    {
        #region Primary properties
        public int Id { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }

        public bool IsTaxable { get; set; }
        #endregion

        #region Foreign keys
        public int PayrollId { get; set; }

        public int DeductionTypeId { get; set; }

        public string? DeductionTypeName { get; set; }

        public int? PayrollPolicyId { get; set; }
        #endregion
    }
}