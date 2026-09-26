using InventorySvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace InventorySvc.DAL.Data
{
    public class InventoryDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Inventory> Inventory { get; set; }
    }
}