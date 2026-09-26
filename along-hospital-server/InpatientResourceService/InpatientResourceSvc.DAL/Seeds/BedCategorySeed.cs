using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Commons.Settings;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class BedCategorySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<BedCategory>().HasData(
                new BedCategory
                {
                    Id = 1,
                    Code = MedicalServiceCodeConstants.STANDARD_BED_CHARGE_CODE,
                    Name = "Standard Bed",
                    Description = "Basic manual hospital bed",
                    CreationDate = seedCreationDate
                },
                new BedCategory
                {
                    Id = 2,
                    Code = MedicalServiceCodeConstants.ELECTRIC_BED_CHARGE_CODE,
                    Name = "Electric Bed",
                    Description = "Motorized adjustable hospital bed with remote control",
                    CreationDate = seedCreationDate
                },
                new BedCategory
                {
                    Id = 3,
                    Code = MedicalServiceCodeConstants.ICU_BED_CHARGE_CODE,
                    Name = "ICU Bed",
                    Description = "Advanced ICU bed with monitoring capabilities",
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}