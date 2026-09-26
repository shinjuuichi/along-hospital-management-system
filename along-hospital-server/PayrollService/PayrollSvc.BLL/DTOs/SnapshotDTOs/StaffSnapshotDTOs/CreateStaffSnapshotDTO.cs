using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.SnapshotDTOs.StaffSnapshotDTOs
{
    public class CreateStaffSnapshotDTO : MapTo<StaffSnapshot>
    {
        public int DependentQuantity { get; set; }
    }
}
