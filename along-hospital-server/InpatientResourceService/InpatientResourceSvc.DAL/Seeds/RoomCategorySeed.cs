using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class RoomCategorySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<RoomCategory>().HasData(
                new RoomCategory
                {
                    Id = 1,
                    Name = "Standard Room",
                    Description = "Basic hospital room with standard facilities",
                    CreationDate = seedCreationDate
                },
                new RoomCategory
                {
                    Id = 2,
                    Name = "VIP Room",
                    Description = "Premium room with advanced amenities and privacy",
                    CreationDate = seedCreationDate
                },
                new RoomCategory
                {
                    Id = 3,
                    Name = "ICU",
                    Description = "Intensive Care Unit for critical patients",
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}