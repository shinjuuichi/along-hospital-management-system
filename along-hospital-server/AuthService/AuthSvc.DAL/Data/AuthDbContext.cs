using AuthSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace AuthSvc.DAL.Data
{
    public class AuthDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<AuthAccount> AuthAccount { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}
