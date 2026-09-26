using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.RegionalWageDTOs
{
    public class UpdateRegionalWageDTO : MapTo<RegionalWage>
    {
        public int Code { get; set; }

        public double MonthlyWage { get; set; }
    }
}