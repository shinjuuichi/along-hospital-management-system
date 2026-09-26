using AttendanceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AttendanceSvc.BLL.DTOs
{
    public class GetAttendanceDTO : MapFrom<Attendance>
    {
        public int Id { get; set; }

        public string? LogType { get; set; }

        public DateTime LogTime { get; set; }

        public int StaffId { get; set; }

        public GetAttendanceStaffDTO? Staff { get; set; }
    }
}
