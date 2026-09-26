using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class MedicineSKUSeed : ISeedBuilder
    {
        public int Priority => 6;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineSKU>().HasData(
                // Paracetamol 500 - Tablet: 3 SKU variants
                new MedicineSKU
                {
                    Id = 1,
                    MedicineId = 1,
                    SKUCode = "PA1BO2050",
                    Name = "Box 20 tablets",
                    Price = 1.50,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicineSKU
                {
                    Id = 2,
                    MedicineId = 1,
                    SKUCode = "PA1BO3050",
                    Name = "Box 30 tablets",
                    Price = 2.00,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicineSKU
                {
                    Id = 3,
                    MedicineId = 1,
                    SKUCode = "PA1BL1250",
                    Name = "Blister 12 tablets",
                    Price = 0.80,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // Amoxicillin 500 - Capsule: 2 SKU variants
                new MedicineSKU
                {
                    Id = 4,
                    MedicineId = 2,
                    SKUCode = "AM2BO2050",
                    Name = "Box 20 capsules",
                    Price = 2.50,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicineSKU
                {
                    Id = 5,
                    MedicineId = 2,
                    SKUCode = "AM2BL1050",
                    Name = "Blister 10 capsules",
                    Price = 1.20,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // Vitamin C 1000 - Effervescent: 2 SKU variants
                new MedicineSKU
                {
                    Id = 6,
                    MedicineId = 3,
                    SKUCode = "VC3BO2050",
                    Name = "Box 20 tablets",
                    Price = 3.00,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicineSKU
                {
                    Id = 7,
                    MedicineId = 3,
                    SKUCode = "VC3BO2010",
                    Name = "Box 30 tablets",
                    Price = 4.50,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // Amoxicillin Syrup - Syrup: 2 SKU variants
                new MedicineSKU
                {
                    Id = 8,
                    MedicineId = 4,
                    SKUCode = "AM3BO3010",
                    Name = "Bottle 30ml",
                    Price = 4.00,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new MedicineSKU
                {
                    Id = 9,
                    MedicineId = 4,
                    SKUCode = "AM3BO3260",
                    Name = "Bottle 60ml",
                    Price = 6.50,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // Bivolet Eye Drops - Eye Drops: 1 SKU
                new MedicineSKU
                {
                    Id = 10,
                    MedicineId = 5,
                    SKUCode = "BI5BO1050",
                    Name = "Bottle 10ml",
                    Price = 3.50,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // Diprosone Ointment - Ointment: 1 SKU
                new MedicineSKU
                {
                    Id = 11,
                    MedicineId = 6,
                    SKUCode = "DI6BO2050",
                    Name = "Box 20 units",
                    Price = 2.80,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // NaCl 0.9% Infusion - Injection 250ml + 50mg/ml
                new MedicineSKU
                {
                    Id = 12,
                    MedicineId = 4,
                    SKUCode = "NA4BO2550",
                    Name = "Bottle 250ml",
                    Price = 5.00,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                // NaCl 0.9% Infusion - Injection 500ml + 100mg/ml
                new MedicineSKU
                {
                    Id = 13,
                    MedicineId = 7,
                    SKUCode = "NA7BO5010",
                    Name = "Bottle 500ml",
                    Price = 12.00,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },

                // Oresol Powder - Powder: 1 SKU
                new MedicineSKU
                {
                    Id = 14,
                    MedicineId = 8,
                    SKUCode = "OR8BO2032",
                    Name = "Box 20 sachets",
                    Price = 0.50,
                    IsActive = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                }
            );

            return modelBuilder;
        }
    }
}
