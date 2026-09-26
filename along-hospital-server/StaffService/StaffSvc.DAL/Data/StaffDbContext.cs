using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Data
{
    public class StaffDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffCertificateType> StaffCertificateType { get; set; }
        public DbSet<StaffCertificate> StaffCertificate { get; set; }
        public DbSet<StaffContract> StaffContract { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Specialty> Specialty { get; set; }
        public DbSet<RegionalWage> RegionalWage { get; set; }
    }
}
