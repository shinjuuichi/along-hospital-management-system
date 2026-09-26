using Microsoft.EntityFrameworkCore;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Data.SqlServerDb;

namespace ProductSvc.DAL.Data
{
    public class ProductDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
