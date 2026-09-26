using BlogSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace BlogSvc.DAL.Seeds
{
    public class BlogCategorySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogCategory>().HasData(
                new BlogCategory
                {
                    Id = 1,
                    Name = "Health",
                    Description = "Health-related articles and tips"
                },
                new BlogCategory
                {
                    Id = 2,
                    Name = "News",
                    Description = "Latest news and updates"
                },
                new BlogCategory
                {
                    Id = 3,
                    Name = "Promotion",
                    Description = "Promotional offers and discounts"
                },
                new BlogCategory
                {
                    Id = 4,
                    Name = "Guide",
                    Description = "Guides and tutorials"
                },
                new BlogCategory
                {
                    Id = 5,
                    Name = "Other",
                    Description = "Other blog posts"
                }
            );

            return modelBuilder;
        }
    }
}
