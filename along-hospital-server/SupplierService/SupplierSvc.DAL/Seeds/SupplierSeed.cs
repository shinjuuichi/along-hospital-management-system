using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.DAL.Seeds
{
    public class SupplierSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            DateTime SeedDate = new(2025, 1, 1);
            modelBuilder.Entity<Supplier>().HasData(
                new Supplier
                {
                    Id = 1,
                    Name = "MediSupply Co",
                    Phone = "0987284222",
                    Email = "contact@medisupply.example",
                    Address = "123 Health St, Wellness City",
                    Note = "Primary supplier for general medicines",
                    CreationDate = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                },
                new Supplier
                {
                    Id = 2,
                    Name = "PharmaDirect Ltd",
                    Phone = "0987282222",
                    Email = "sales@pharmadirect.example",
                    Address = "45 Pharmacy Ave, Caretown",
                    Note = "Fast delivery partner",
                    CreationDate = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                },
                new Supplier
                {
                    Id = 3,
                    Name = "HealthPlus Distributors",
                    Phone = "09872382211",
                    Email = "support@healthplus.example",
                    Address = "9 Clinic Rd, Healborough",
                    Note = "Specializes in OTC products",
                    CreationDate = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                }
            );

            return modelBuilder;
        }
    }
}
