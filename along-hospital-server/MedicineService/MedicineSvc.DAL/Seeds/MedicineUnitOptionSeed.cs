using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class MedicineUnitOptionSeed : ISeedBuilder
    {
        public int Priority => 4;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineUnitOption>().HasData(
                new MedicineUnitOption { MedicineUnitId = 1, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 1, OptionId = 2, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 1, OptionId = 3, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 2, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 2, OptionId = 2, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 2, OptionId = 3, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 3, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 3, OptionId = 4, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 3, OptionId = 5, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 4, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 4, OptionId = 4, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 4, OptionId = 3, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 5, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 5, OptionId = 4, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 5, OptionId = 5, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 6, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 6, OptionId = 2, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 6, OptionId = 5, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 7, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 7, OptionId = 2, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 7, OptionId = 3, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 8, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 8, OptionId = 4, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 8, OptionId = 5, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 9, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 9, OptionId = 2, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 9, OptionId = 3, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 10, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 10, OptionId = 4, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 10, OptionId = 5, IsActive = true },

                new MedicineUnitOption { MedicineUnitId = 11, OptionId = 1, IsActive = true },
                new MedicineUnitOption { MedicineUnitId = 11, OptionId = 2, IsActive = true }
            );

            return modelBuilder;
        }
    }
}
