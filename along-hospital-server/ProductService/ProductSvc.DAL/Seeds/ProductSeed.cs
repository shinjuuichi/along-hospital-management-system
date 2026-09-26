using Microsoft.EntityFrameworkCore;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace ProductSvc.DAL.Seeds
{
    public class ProductSeed : ISeedBuilder
    {
        public int Priority => 11;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Product>().HasData(
                new()
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description for Product 1",
                    Price = 10.0,
                    CategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new()
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Description for Product 2",
                    Price = 20.0,
                    CategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new()
                {
                    Id = 3,
                    Name = "Product 3",
                    Description = "Description for Product 3",
                    Price = 30.0,
                    CategoryId = 3,
                    CreationDate = seedCreationDate
                }
            );
            return modelBuilder;
        }
    }
}
