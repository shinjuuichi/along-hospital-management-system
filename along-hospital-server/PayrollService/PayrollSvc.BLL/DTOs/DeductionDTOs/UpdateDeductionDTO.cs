using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.DeductionDTOs
{
    public class UpdateDeductionDTO : MapTo<Deduction>
    {
        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
    }
}