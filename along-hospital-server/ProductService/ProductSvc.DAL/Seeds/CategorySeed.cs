using Microsoft.EntityFrameworkCore;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace ProductSvc.DAL.Seeds
{
    public class CategorySeed : ISeedBuilder
    {
        public int Priority => 10;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Category>().HasData(
                new()
                {
                    Id = 1,
                    Name = "Category 1",
                    CreationDate = seedCreationDate
                },
                new()
                {
                    Id = 2,
                    Name = "Category 2",
                    CreationDate = seedCreationDate
                },
                new()
                {
                    Id = 3,
                    Name = "Category 3",
                    CreationDate = seedCreationDate
                }
            );
            return modelBuilder;

        }
    }
}
