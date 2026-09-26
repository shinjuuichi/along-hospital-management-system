using BlogSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace BlogSvc.DAL.Data
{
    public class BlogDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Blog> Blog { get; set; }
        public DbSet<BlogCategory> BlogCategory { get; set; }
    }
}