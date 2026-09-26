using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffContractDTOs
{
    public class CreateStaffContractDTO : MapTo<StaffContract>
    {
        public string? ContractType { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public double HourlyRate { get; set; }

        public int WorkingHoursPerWeek { get; set; }

        public string? Status { get; set; }

        public double InsuranceSalaryRate { get; set; }

        public int StaffId { get; set; }

        public int RegionalWageId { get; set; }
    }
}
