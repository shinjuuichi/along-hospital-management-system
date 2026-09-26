namespace AttendanceSvc.BLL.DTOs
{
    public class GetAttendanceLogsByStaffsRangeDTO
    {
        public List<int> StaffIds { get; set; } = [];

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}