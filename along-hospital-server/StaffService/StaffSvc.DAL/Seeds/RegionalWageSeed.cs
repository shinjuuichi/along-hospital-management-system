using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class RegionalWageSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<RegionalWage>().HasData(
                new RegionalWage
                {
                    Id = 1,
                    Code = 1,
                    MonthlyWage = 5_000_000,
                    CreationDate = seedCreationDate
                },
                new RegionalWage
                {
                    Id = 2,
                    Code = 2,
                    MonthlyWage = 4_500_000,
                    CreationDate = seedCreationDate
                },
                new RegionalWage
                {
                    Id = 3,
                    Code = 3,
                    MonthlyWage = 4_000_000,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
