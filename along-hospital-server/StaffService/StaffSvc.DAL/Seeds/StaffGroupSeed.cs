using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class StaffGroupSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<StaffGroup>().HasData(
                new StaffGroup
                {
                    Id = 1,
                    Name = "Doctors",
                    CreationDate = seedDate
                },
                new StaffGroup
                {
                    Id = 2,
                    Name = "Nurses",
                    CreationDate = seedDate
                },
                new StaffGroup
                {
                    Id = 3,
                    Name = "TeleHealth Staff",
                    CreationDate = seedDate
                }
            );

            return modelBuilder;
        }
    }
}
