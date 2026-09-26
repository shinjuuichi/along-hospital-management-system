using AttendanceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AttendanceSvc.BLL.DTOs
{
    public class CreateAttendanceDTO : MapTo<Attendance>
    {
        public string? LogType { get; set; }

        public int StaffId { get; set; }
    }
}
