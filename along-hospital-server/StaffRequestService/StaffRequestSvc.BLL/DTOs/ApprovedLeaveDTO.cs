namespace StaffRequestSvc.BLL.DTOs
{
    public class ApprovedLeaveDTO
    {
        public int StaffId { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public int? ShiftId { get; set; }

        public string? LeaveUnit { get; set; }
    }
}