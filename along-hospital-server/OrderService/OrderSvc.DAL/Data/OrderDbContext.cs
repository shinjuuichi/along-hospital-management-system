using Microsoft.EntityFrameworkCore;
using OrderSvc.DAL.Models;
using SharedLibrary.Base.Data.SqlServerDb;

namespace OrderSvc.DAL.Data
{
    public class OrderDbContext(DbContextOptions dbContext) : BaseDbContext(dbContext)
    {
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}
