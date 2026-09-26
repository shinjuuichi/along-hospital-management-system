using SharedLibrary.Base.Mappers;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.DTOs
{
    public class GetApprovedLeavesByStaffsRangeDTO : MapFrom<LeaveRequest>
    {
        public List<int> StaffIds { get; set; } = [];

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }
    }
}