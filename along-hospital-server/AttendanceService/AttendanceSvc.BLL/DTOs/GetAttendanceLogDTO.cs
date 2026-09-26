using AttendanceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AttendanceSvc.BLL.DTOs
{
    public class GetAttendanceLogDTO : MapFrom<Attendance>
    {
        public int StaffId { get; set; }

        public DateTime LogTime { get; set; }

        public string? LogType { get; set; }
    }
}