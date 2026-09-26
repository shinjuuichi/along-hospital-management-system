using PayrollSvc.DAL.Models;

namespace PayrollSvc.BLL.DTOs.DeductionDTOs
{
    public class DeductionResultDTO
    {
        public List<Deduction> Deductions { get; set; } = [];

        public double TotalAmount { get; set; }

        public double TaxableAmount { get; set; }
    }
}