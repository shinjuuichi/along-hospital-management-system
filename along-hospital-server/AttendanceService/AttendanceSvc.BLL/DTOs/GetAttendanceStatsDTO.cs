namespace AttendanceSvc.BLL.DTOs
{
    public class GetAttendanceStatsDTO
    {
        public bool CanCheckIn { get; set; }
        public bool CanCheckOut { get; set; }
    }
}