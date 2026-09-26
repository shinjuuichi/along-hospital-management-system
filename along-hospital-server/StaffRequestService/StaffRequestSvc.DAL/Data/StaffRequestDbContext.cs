using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.DAL.Data
{
    public class StaffRequestDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<LeaveRequest> LeaveRequest { get; set; }
        public DbSet<SalaryAdvance> SalaryAdvance { get; set; }
    }
}