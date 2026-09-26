using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;
using UserSvc.DAL.Models;

namespace UserSvc.DAL.Data
{
    public class UserDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<User> User { get; set; }
    }
}