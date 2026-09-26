using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class OptionSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>().HasData(
                new Option { Id = 1, OptionName = "PackagingType" },
                new Option { Id = 2, OptionName = "PackQuantity" },
                new Option { Id = 3, OptionName = "Dosage" },
                new Option { Id = 4, OptionName = "Volume" },
                new Option { Id = 5, OptionName = "Concentration" }
            );

            return modelBuilder;
        }
    }
}
