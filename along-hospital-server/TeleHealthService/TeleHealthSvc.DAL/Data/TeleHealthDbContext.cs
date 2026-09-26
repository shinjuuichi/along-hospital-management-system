using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.DAL.Data
{
    public class TeleHealthDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<TeleSession> TeleSession { get; set; }
        public DbSet<TeleRoom> TeleRoom { get; set; }
    }
}