using Microsoft.EntityFrameworkCore;
using PatientSvc.DAL.Models;
using SharedLibrary.Base.Data.SqlServerDb;

namespace PatientSvc.DAL.Data
{
    public class PatientDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Allergy> Allergy { get; set; }
    }
}
