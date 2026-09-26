using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class FloorSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Floor>().HasData(
                new Floor
                {
                    Id = 1,
                    FloorNumber = 1,
                    BuildingId = 1,
                    CreationDate = seedCreationDate
                },
                new Floor
                {
                    Id = 2,
                    FloorNumber = 2,
                    BuildingId = 1,
                    CreationDate = seedCreationDate
                },
                new Floor
                {
                    Id = 3,
                    FloorNumber = 1,
                    BuildingId = 2,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}