using AttendanceSvc.DAL.Enums;
using AttendanceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace AttendanceSvc.DAL.Seeds
{
    public class AttendanceSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var baseDate = new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Attendance>().HasData(
                new Attendance
                {
                    Id = 1,
                    StaffId = 4,
                    LogType = AttendanceLogTypeEnum.CheckIn,
                    LogTime = baseDate.AddHours(8)
                },
                new Attendance
                {
                    Id = 2,
                    StaffId = 4,
                    LogType = AttendanceLogTypeEnum.CheckOut,
                    LogTime = baseDate.AddHours(12)
                },
                new Attendance
                {
                    Id = 3,
                    StaffId = 4,
                    LogType = AttendanceLogTypeEnum.CheckOut,
                    LogTime = baseDate.AddHours(17)
                },

                new Attendance
                {
                    Id = 4,
                    StaffId = 5,
                    LogType = AttendanceLogTypeEnum.CheckIn,
                    LogTime = baseDate.AddHours(8)
                },
                new Attendance
                {
                    Id = 5,
                    StaffId = 5,
                    LogType = AttendanceLogTypeEnum.CheckOut,
                    LogTime = baseDate.AddHours(12)
                },
                new Attendance
                {
                    Id = 6,
                    StaffId = 5,
                    LogType = AttendanceLogTypeEnum.CheckOut,
                    LogTime = baseDate.AddHours(17)
                }
            );
            return modelBuilder;
        }
    }
}
