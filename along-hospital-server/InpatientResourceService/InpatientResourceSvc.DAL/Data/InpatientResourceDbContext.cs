using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace InpatientResourceSvc.DAL.Data
{
    public class InpatientResourceDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Building> Building { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<RoomCategory> RoomCategory { get; set; }
        public DbSet<RoomCategoryRole> RoomCategoryRole { get; set; }
        public DbSet<RoomCategoryRoleMapping> RoomCategoryRoleMapping { get; set; }
        public DbSet<Bed> Bed { get; set; }
        public DbSet<BedCategory> BedCategory { get; set; }
        public DbSet<BedOccupancy> BedOccupancy { get; set; }
    }
}