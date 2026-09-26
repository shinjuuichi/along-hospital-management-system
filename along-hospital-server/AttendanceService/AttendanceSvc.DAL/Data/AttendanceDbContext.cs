using AttendanceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace AttendanceSvc.DAL.Data
{
    public class AttendanceDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Attendance> Attendance { get; set; }
    }
}
