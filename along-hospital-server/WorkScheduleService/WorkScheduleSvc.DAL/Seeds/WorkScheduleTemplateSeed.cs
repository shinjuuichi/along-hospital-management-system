using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkScheduleTemplateSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<WorkScheduleTemplate>().HasData(
                new WorkScheduleTemplate
                {
                    Id = 1,
                    Name = "April 2026 Template",
                    Description = "Monthly template for April 2026. Each work date in April is linked to this single template.",
                    IsActive = true,
                    CreationDate = seedDate
                },
                new WorkScheduleTemplate
                {
                    Id = 2,
                    Name = "May 2026 Template",
                    Description = "Monthly template for May 2026. Each work date in May is linked to this single template.",
                    IsActive = true,
                    CreationDate = seedDate
                },
                new WorkScheduleTemplate
                {
                    Id = 3,
                    Name = "June 2026 Template",
                    Description = "Prepared monthly template for June 2026 scheduling.",
                    IsActive = false,
                    CreationDate = seedDate
                }
            );

            return modelBuilder;
        }
    }
}
