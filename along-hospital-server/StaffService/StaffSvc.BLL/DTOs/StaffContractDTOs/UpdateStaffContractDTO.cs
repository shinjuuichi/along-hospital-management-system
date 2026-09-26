using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffContractDTOs
{
    public class UpdateStaffContractDTO : MapTo<StaffContract>
    {
        public DateOnly? EndDate { get; set; }

        public double HourlyRate { get; set; }

        public int WorkingHoursPerWeek { get; set; }

        public double InsuranceSalaryRate { get; set; }

        public int RegionalWageId { get; set; }
    }
}
