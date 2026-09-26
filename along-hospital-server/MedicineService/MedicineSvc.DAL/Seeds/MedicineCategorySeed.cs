using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Commons.Settings;

namespace MedicineSvc.DAL.Seeds
{
    public class MedicineCategorySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineCategory>().HasData(
                new MedicineCategory
                {
                    Id = 1,
                    Name = MedicineCategoryNameConstants.ANTIBIOTICS_NAME,
                    Description = "Used to treat bacterial infections."
                },
                new MedicineCategory
                {
                    Id = 2,
                    Name = MedicineCategoryNameConstants.PAINKILLERS_NAME,
                    Description = "Used to relieve pain and reduce fever."
                },
                new MedicineCategory
                {
                    Id = 3,
                    Name = MedicineCategoryNameConstants.VITAMINS_SUPPLEMENTS_NAME,
                    Description = "Used to support health and nutrition."
                },
                new MedicineCategory
                {
                    Id = 4,
                    Name = MedicineCategoryNameConstants.INFUSION_SOLUTIONS_NAME,
                    Description = "Intravenous fluids and infusion solutions for hydration and therapy."
                }
            );

            return modelBuilder;
        }
    }
}