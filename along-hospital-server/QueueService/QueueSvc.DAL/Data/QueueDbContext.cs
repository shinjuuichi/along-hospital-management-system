using Microsoft.EntityFrameworkCore;
using QueueSvc.DAL.Models;
using SharedLibrary.Base.Data.SqlServerDb;

namespace QueueSvc.DAL.Data
{
    public class QueueDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Queue> Queue { get; set; }
        public DbSet<QueueEvent> QueueEvent { get; set; }
    }
}