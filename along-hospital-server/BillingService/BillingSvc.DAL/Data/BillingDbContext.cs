using BillingSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace BillingSvc.DAL.Data
{
    public class BillingDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Charge> Charge { get; set; }
        public DbSet<Refund> Refund { get; set; }
    }
}