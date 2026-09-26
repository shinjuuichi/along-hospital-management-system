using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SupplierSvc.DAL.Models;
using SupplierSvc.DAL.Models.Snapshots;

namespace SupplierSvc.DAL.Seeds
{
    public class ImportRequestDetailSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            DateTime SeedDate = new(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<ImportRequestDetail>().HasData(
                new ImportRequestDetail
                {
                    Id = 1,
                    ImportRequestId = 1,
                    SKUCode = "PA1BO2050",
                    RequestQuantity = 100,
                    CreationDate = SeedDate,
                    CreatedBy = 13,
                    MedicineSnapshot = new ImportRequestDetailSnapshot { MedicineName = "Paracetamol", UnitPrice = 100 }
                },
                new ImportRequestDetail
                {
                    Id = 2,
                    ImportRequestId = 2,
                    SKUCode = "AM2BO2050",
                    RequestQuantity = 200,
                    CreationDate = SeedDate,
                    CreatedBy = 13,
                    MedicineSnapshot = new ImportRequestDetailSnapshot { MedicineName = "Amoxicillin", UnitPrice = 100 }
                },
                new ImportRequestDetail
                {
                    Id = 3,
                    ImportRequestId = 3,
                    SKUCode = "VC3BO2010",
                    RequestQuantity = 300,
                    CreationDate = SeedDate,
                    CreatedBy = 13,
                    MedicineSnapshot = new ImportRequestDetailSnapshot { MedicineName = "Vitamin C", UnitPrice = 100 }
                },
                new ImportRequestDetail
                {
                    Id = 4,
                    ImportRequestId = 4,
                    SKUCode = "PA1BO3050",
                    RequestQuantity = 150,
                    CreationDate = SeedDate.AddDays(5),
                    CreatedBy = 13,
                    MedicineSnapshot = new ImportRequestDetailSnapshot { MedicineName = "Paracetamol" }
                }
            );

            return modelBuilder;
        }
    }
}
