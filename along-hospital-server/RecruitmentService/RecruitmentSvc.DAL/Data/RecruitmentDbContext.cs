using Microsoft.EntityFrameworkCore;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data.SqlServerDb;

namespace RecruitmentSvc.DAL.Data
{
    public class RecruitmentDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<JobPosting> JobPosting { get; set; }
        public DbSet<JobApplication> JobApplication { get; set; }
        public DbSet<Interview> Interview { get; set; }
        public DbSet<InterviewType> InterviewType { get; set; }
    }
}
