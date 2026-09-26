using MedicineSvc.DAL.Enums;
using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class MedicineSeed : ISeedBuilder
    {
        public int Priority => 5;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine
                {
                    Id = 1,
                    Name = "Paracetamol 500",
                    Brand = "Stada",
                    MedicineUnitId = 1,
                    MedicineCategoryId = 2,
                    Images = ["Medicine/thuoc-paracetamol-650-mg-mediplantex-3-c1880_1770294316_bdee440d.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 2,
                    Name = "Amoxicillin 500",
                    Brand = "Medipharma",
                    MedicineUnitId = 2,
                    MedicineCategoryId = 1,
                    Images = ["Medicine/amoxicillin_1770294305_24f46c57.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = false,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 3,
                    Name = "Vitamin C 1000",
                    Brand = "Nature's Way",
                    MedicineUnitId = 9,
                    MedicineCategoryId = 3,
                    Images = ["Medicine/VitaminC_1770294282_a543fa89.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 4,
                    Name = "Amoxicillin Syrup",
                    Brand = "Medipharma",
                    MedicineUnitId = 3,
                    MedicineCategoryId = 1,
                    Images = ["Medicine/001f8ae49f5449feaed195262defce06.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = false,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 5,
                    Name = "Bivolet Eye Drops",
                    Brand = "Optimax",
                    MedicineUnitId = 5,
                    MedicineCategoryId = 1,
                    Images = ["Medicine/cbd27a962c2c4107bf8a4adcf97f0a75.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 6,
                    Name = "Diprosone Ointment",
                    Brand = "Schering",
                    MedicineUnitId = 6,
                    MedicineCategoryId = 2,
                    Images = ["Medicine/808d7e699e5a4d4cbcdd8eeeadcbebf5.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = false,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 7,
                    Name = "NaCl 0.9% Infusion",
                    Brand = "B Braun",
                    MedicineUnitId = 4,
                    MedicineCategoryId = 4,
                    Images = ["Medicine/1b6f35f7a68d4bea831ae57255ed1f6a.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = false,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 8,
                    Name = "Oresol Powder",
                    Brand = "Vimed",
                    MedicineUnitId = 7,
                    MedicineCategoryId = 2,
                    Images = ["Medicine/08f0a64c6ca840d0a0f29d3ed5260ca7.webp"],
                    Status = MedicineStatusEnum.Active,
                    IsPublic = true,
                    CreationDate = new DateTime(2025, 01, 01),
                    IsDeleted = false
                }
            );

            return modelBuilder;
        }
    }
}
