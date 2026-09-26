using FeedbackSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace FeedbackSvc.DAL.Data
{
    public class FeedbackDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<FeedbackReport> FeedbackReport { get; set; }
        public DbSet<FeedbackRespond> FeedbackRespond { get; set; }
    }
}