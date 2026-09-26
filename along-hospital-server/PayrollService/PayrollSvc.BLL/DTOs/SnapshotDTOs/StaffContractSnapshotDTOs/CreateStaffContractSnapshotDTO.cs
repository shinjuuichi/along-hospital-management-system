using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.SnapshotDTOs.StaffContractSnapshotDTOs
{
    public class CreateStaffContractSnapshotDTO : MapTo<StaffContractSnapshot>
    {
        public double HourlyRate { get; set; }

        public double InsuranceSalaryRate { get; set; }
    }
}
