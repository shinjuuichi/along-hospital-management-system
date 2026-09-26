using SharedLibrary.Base.Mappers;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs
{
    public class CreateSalaryAdvanceDTO : MapTo<SalaryAdvance>
    {
        public double Amount { get; set; }

        public string? Reason { get; set; }
    }
}