using AppointmentSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace AppointmentSvc.DAL.Seeds
{
    public class TimeSlotSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDateTime = new DateTime(2026, 1, 1);

            modelBuilder.Entity<TimeSlot>().HasData(
                // Morning slots
                new TimeSlot { Id = 1, Time = new TimeOnly(7, 0), CapacityPerDoctor = 4, CreationDate = seedDateTime },
                new TimeSlot { Id = 2, Time = new TimeOnly(7, 30), CapacityPerDoctor = 4, CreationDate = seedDateTime },
                new TimeSlot { Id = 3, Time = new TimeOnly(8, 0), CapacityPerDoctor = 3, CreationDate = seedDateTime },
                new TimeSlot { Id = 4, Time = new TimeOnly(8, 30), CapacityPerDoctor = 3, CreationDate = seedDateTime },
                new TimeSlot { Id = 5, Time = new TimeOnly(9, 0), CapacityPerDoctor = 2, CreationDate = seedDateTime },
                new TimeSlot { Id = 6, Time = new TimeOnly(9, 30), CapacityPerDoctor = 2, CreationDate = seedDateTime },
                new TimeSlot { Id = 7, Time = new TimeOnly(10, 0), CapacityPerDoctor = 1, CreationDate = seedDateTime },
                new TimeSlot { Id = 8, Time = new TimeOnly(10, 30), CapacityPerDoctor = 1, CreationDate = seedDateTime },

                // Afternoon slots
                new TimeSlot { Id = 9, Time = new TimeOnly(13, 0), CapacityPerDoctor = 4, CreationDate = seedDateTime },
                new TimeSlot { Id = 10, Time = new TimeOnly(13, 30), CapacityPerDoctor = 4, CreationDate = seedDateTime },
                new TimeSlot { Id = 11, Time = new TimeOnly(14, 0), CapacityPerDoctor = 3, CreationDate = seedDateTime },
                new TimeSlot { Id = 12, Time = new TimeOnly(14, 30), CapacityPerDoctor = 3, CreationDate = seedDateTime },
                new TimeSlot { Id = 13, Time = new TimeOnly(15, 0), CapacityPerDoctor = 2, CreationDate = seedDateTime },
                new TimeSlot { Id = 14, Time = new TimeOnly(15, 30), CapacityPerDoctor = 2, CreationDate = seedDateTime },
                new TimeSlot { Id = 15, Time = new TimeOnly(16, 0), CapacityPerDoctor = 1, CreationDate = seedDateTime },
                new TimeSlot { Id = 16, Time = new TimeOnly(16, 30), CapacityPerDoctor = 1, CreationDate = seedDateTime }
            );

            return modelBuilder;
        }
    }
}
