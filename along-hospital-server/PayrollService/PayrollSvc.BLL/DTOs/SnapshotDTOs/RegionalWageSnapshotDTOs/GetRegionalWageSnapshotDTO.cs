using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.SnapshotDTOs.RegionalWageSnapshotDTOs
{
    public class GetRegionalWageSnapshotDTO : MapFrom<RegionalWageSnapshot>
    {
        public double MonthlyWage { get; set; }

        public int Region { get; set; }
    }
}