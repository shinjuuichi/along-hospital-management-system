using MedicalHistorySvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace MedicalHistorySvc.DAL.Data
{
    public class MedicalHistoryDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Complaint> Complaint { get; set; }
        public DbSet<ComplaintSummary> ComplaintSummary { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetail { get; set; }
    }
}