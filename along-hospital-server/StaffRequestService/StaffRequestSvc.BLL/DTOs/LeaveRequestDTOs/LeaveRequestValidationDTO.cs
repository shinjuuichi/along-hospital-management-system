namespace StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs
{
    public class LeaveRequestValidationDTO
    {
        public int StaffId { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public string LeaveUnit { get; set; } = string.Empty;

        public int? ShiftId { get; set; }
    }
}
