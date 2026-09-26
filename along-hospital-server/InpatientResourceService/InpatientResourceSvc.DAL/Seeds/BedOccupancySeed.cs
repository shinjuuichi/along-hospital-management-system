using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class BedOccupancySeed : ISeedBuilder
    {
        public int Priority => 4;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<BedOccupancy>().HasData(
                new BedOccupancy
                {
                    Id = 1,
                    FromDateTime = new DateTime(2026, 1, 31),
                    ToDateTime = null,
                    OccupancyStatus = OccupancyStatusEnum.Active,
                    MedicalHistoryId = 3,
                    BedId = 2,
                    CreationDate = seedCreationDate
                },
                new BedOccupancy
                {
                    Id = 2,
                    FromDateTime = new DateTime(2026, 2, 5),
                    ToDateTime = null,
                    OccupancyStatus = OccupancyStatusEnum.Active,
                    MedicalHistoryId = 4,
                    BedId = 3,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}