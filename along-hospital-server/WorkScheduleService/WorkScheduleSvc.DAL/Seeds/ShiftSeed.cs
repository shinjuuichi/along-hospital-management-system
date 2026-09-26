using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class ShiftSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shift>().HasData(
                new Shift
                {
                    Id = 1,
                    Name = "Morning",
                    StartTime = new TimeOnly(7, 0),
                    EndTime = new TimeOnly(12, 0)
                },
                new Shift
                {
                    Id = 2,
                    Name = "Afternoon",
                    StartTime = new TimeOnly(13, 0),
                    EndTime = new TimeOnly(17, 0)
                },
                new Shift
                {
                    Id = 3,
                    Name = "Evening",
                    StartTime = new TimeOnly(19, 0),
                    EndTime = new TimeOnly(20, 30),
                    IsOvertime = true

                },
                new Shift
                {
                    Id = 4,
                    Name = "Night",
                    StartTime = new TimeOnly(22, 0),
                    EndTime = new TimeOnly(23, 30),
                    IsOvertime = true
                }
            );

            return modelBuilder;
        }
    }
}
