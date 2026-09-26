using AttendanceSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;

namespace AttendanceSvc.DAL.Models
{
    public class Attendance : BaseEntity
    {
        public AttendanceLogTypeEnum LogType { get; set; } = AttendanceLogTypeEnum.CheckIn;

        public DateTime LogTime { get; set; } = DateTime.UtcNow;

        public int StaffId { get; set; }
    }
}
