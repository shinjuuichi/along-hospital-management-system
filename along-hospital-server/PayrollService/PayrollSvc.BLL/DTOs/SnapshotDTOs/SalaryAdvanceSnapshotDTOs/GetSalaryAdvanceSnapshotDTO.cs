using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.SnapshotDTOs.SalaryAdvanceSnapshotDTOs
{
    public class GetSalaryAdvanceSnapshotDTO : MapFrom<SalaryAdvanceSnapshot>
    {
        public double Amount { get; set; }

        public string? Reason { get; set; }
    }
}
