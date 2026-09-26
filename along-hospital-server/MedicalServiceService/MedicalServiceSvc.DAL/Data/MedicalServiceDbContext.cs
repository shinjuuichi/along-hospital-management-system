using MedicalServiceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace MedicalServiceSvc.DAL.Data
{
    public class MedicalServiceDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<MedicalService> MedicalService { get; set; }
        public DbSet<MedicalServiceRole> MedicalServiceRole { get; set; }
    }
}