using SharedLibrary.Base.Mappers;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs
{
    public class GetSalaryAdvanceDTO : MapFrom<SalaryAdvance>
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public string? Reason { get; set; }

        public string? Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public int? PayrollId { get; set; }
    }
}