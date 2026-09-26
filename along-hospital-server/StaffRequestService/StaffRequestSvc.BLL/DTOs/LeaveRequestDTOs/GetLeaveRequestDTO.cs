using SharedLibrary.Base.Mappers;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs
{
    public class GetLeaveRequestDTO : MapFrom<LeaveRequest>
    {
        public int Id { get; set; }

        public string? LeaveType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public string? Reason { get; set; }

        public string? Status { get; set; }

        public DateTime? DecidedAt { get; set; }

        public string? LeaveUnit { get; set; }

        public int? ShiftId { get; set; }

        public string? ShiftName { get; set; }

        public TimeOnly? ShiftStartTime { get; set; }

        public TimeOnly? ShiftEndTime { get; set; }

        public UserInfoDTO? Staff { get; set; }

        public UserInfoDTO? Decider { get; set; }
    }
}