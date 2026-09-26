using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.DAL.Data
{
    public class SupplierDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Import> Import { get; set; }
        public DbSet<ImportDetail> ImportDetail { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<ImportRequest> ImportRequest { get; set; }
        public DbSet<ImportRequestDetail> ImportRequestDetail { get; set; }
    }
}