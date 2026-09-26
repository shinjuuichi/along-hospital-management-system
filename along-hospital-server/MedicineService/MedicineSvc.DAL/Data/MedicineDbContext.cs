using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace MedicineSvc.DAL.Data
{
    public class MedicineDbContext(DbContextOptions dbContext) : BaseDbContext(dbContext)
    {
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<MedicineCategory> MedicineCategory { get; set; }
        public DbSet<MedicineUnit> MedicineUnit { get; set; }
        public DbSet<MedicineUnitOption> MedicineUnitOption { get; set; }
        public DbSet<Option> Option { get; set; }
        public DbSet<OptionValue> OptionValue { get; set; }
        public DbSet<MedicineSKU> MedicineSKU { get; set; }
        public DbSet<SKUValue> SKUValue { get; set; }
    }
}