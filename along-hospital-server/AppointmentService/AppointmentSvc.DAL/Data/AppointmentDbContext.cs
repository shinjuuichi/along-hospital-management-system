using AppointmentSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;

namespace AppointmentSvc.DAL.Data
{
    public class AppointmentDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
    }
}
