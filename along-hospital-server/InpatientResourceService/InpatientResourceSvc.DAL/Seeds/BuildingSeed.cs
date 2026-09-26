using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class BuildingSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Building>().HasData(
                new Building
                {
                    Id = 1,
                    Name = "Alpha Building",
                    Location = "Main Campus, North Wing",
                    CreationDate = seedCreationDate
                },
                new Building
                {
                    Id = 2,
                    Name = "Beta Building",
                    Location = "Main Campus, South Wing",
                    CreationDate = seedCreationDate
                },
                new Building
                {
                    Id = 3,
                    Name = "Gamma Building",
                    Location = "Emergency Wing, East Side",
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}