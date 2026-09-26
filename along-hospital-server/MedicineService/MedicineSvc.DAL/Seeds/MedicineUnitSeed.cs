using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class MedicineUnitSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineUnit>().HasData(
                new MedicineUnit
                {
                    Id = 1,
                    Name = "Tablet",
                    Description = "Solid oral dosage form pressed into flat shape"
                },
                new MedicineUnit
                {
                    Id = 2,
                    Name = "Capsule",
                    Description = "Solid dosage form enclosed in gelatin shell"
                },
                new MedicineUnit
                {
                    Id = 3,
                    Name = "Syrup",
                    Description = "Sweet liquid oral medication"
                },
                new MedicineUnit
                {
                    Id = 4,
                    Name = "Injection",
                    Description = "Sterile solution for parenteral administration"
                },
                new MedicineUnit
                {
                    Id = 5,
                    Name = "Eye Drops",
                    Description = "Sterile liquid for ophthalmic use"
                },
                new MedicineUnit
                {
                    Id = 6,
                    Name = "Ointment",
                    Description = "Semi-solid topical preparation"
                },
                new MedicineUnit
                {
                    Id = 7,
                    Name = "Powder",
                    Description = "Fine dry particles for oral or topical use"
                },
                new MedicineUnit
                {
                    Id = 8,
                    Name = "Suspension",
                    Description = "Liquid with undissolved particles"
                },
                new MedicineUnit
                {
                    Id = 9,
                    Name = "Effervescent",
                    Description = "Soluble tablet that fizzes when dissolved"
                },
                new MedicineUnit
                {
                    Id = 10,
                    Name = "Spray",
                    Description = "Aerosol or pump dispenser for topical or oral use"
                },
                new MedicineUnit
                {
                    Id = 11,
                    Name = "Suppository",
                    Description = "Solid dosage form for rectal or vaginal insertion"
                }
            );

            return modelBuilder;
        }
    }
}
