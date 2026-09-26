using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data.SqlServerDb;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Data
{
    public class WorkScheduleDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Shift> Shift { get; set; }
        public DbSet<Holiday> Holiday { get; set; }
        public DbSet<WorkSchedule> WorkSchedule { get; set; }
        public DbSet<WorkScheduleAssignment> WorkScheduleAssignment { get; set; }
        public DbSet<WorkSegment> WorkSegment { get; set; }
        public DbSet<WorkScheduleTemplate> WorkScheduleTemplate { get; set; }
        public DbSet<WorkScheduleTemplateDayShift> WorkScheduleTemplateDayShift { get; set; }
        public DbSet<WorkScheduleTemplateAssignmentForStaffRoom> WorkScheduleTemplateAssignmentForStaffRoom { get; set; }
        public DbSet<WorkScheduleTemplateAssignmentForStaffTeleRoom> WorkScheduleTemplateAssignmentForStaffTeleRoom { get; set; }
    }
}