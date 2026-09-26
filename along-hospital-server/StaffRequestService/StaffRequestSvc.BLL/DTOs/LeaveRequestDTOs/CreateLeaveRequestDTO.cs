using SharedLibrary.Base.Mappers;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs
{
    public class CreateLeaveRequestDTO : MapTo<LeaveRequest>
    {
        public string? LeaveType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public string? Reason { get; set; }

        public string? LeaveUnit { get; set; }

        public int? ShiftId { get; set; }
    }
}