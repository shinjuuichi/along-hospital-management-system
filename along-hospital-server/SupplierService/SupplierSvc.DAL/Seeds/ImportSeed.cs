using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.DAL.Seeds
{
    public class ImportSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            DateTime SeedDate = new(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Import>().HasData(
                new Import
                {
                    Id = 1,
                    ImportDate = SeedDate,
                    Note = "Seed Import 1",
                    ManagerId = 1,
                    SupplierId = 1,
                    ImportRequestId = 1,
                    CreationDate = SeedDate,
                },
                new Import
                {
                    Id = 2,
                    ImportDate = SeedDate,
                    Note = "Seed Import 2",
                    ManagerId = 2,
                    SupplierId = 2,
                    ImportRequestId = 2,
                    CreationDate = SeedDate,
                },
                new Import
                {
                    Id = 3,
                    ImportDate = SeedDate,
                    Note = "Seed Import 3",
                    ManagerId = 3,
                    SupplierId = 3,
                    ImportRequestId = 3,
                    CreationDate = SeedDate,
                }
            );

            return modelBuilder;
        }
    }
}