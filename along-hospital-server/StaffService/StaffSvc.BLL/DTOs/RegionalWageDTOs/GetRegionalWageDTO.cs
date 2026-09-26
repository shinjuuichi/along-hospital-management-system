using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.RegionalWageDTOs
{
    public class GetRegionalWageDTO : MapFrom<RegionalWage>
    {
        public int Id { get; set; }

        public int Code { get; set; }

        public double MonthlyWage { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
