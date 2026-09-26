using PayrollSvc.DAL.Models;

namespace PayrollSvc.BLL.DTOs.AllowanceDTOs
{
    public class AllowanceResultDTO
    {
        public List<Allowance> Allowances { get; set; } = [];

        public double TotalAmount { get; set; }

        public double TaxableAmount { get; set; }

        public double InsuranceBasisAmount { get; set; }
    }
}