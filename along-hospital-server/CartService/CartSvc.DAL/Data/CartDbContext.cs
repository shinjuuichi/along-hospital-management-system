using CartSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace CartSvc.DAL.Data
{
    public class CartDbContext(DbContextOptions dbContext) : BaseDbContext(dbContext)
    {
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
    }
}
