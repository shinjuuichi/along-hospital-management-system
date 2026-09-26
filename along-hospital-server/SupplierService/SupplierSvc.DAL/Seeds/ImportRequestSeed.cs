using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SupplierSvc.DAL.Enums;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.DAL.Seeds
{
    public class ImportRequestSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            DateTime SeedDate = new(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<ImportRequest>().HasData(
                new ImportRequest
                {
                    Id = 1,
                    RequestDate = new DateOnly(2026, 3, 3),
                    Status = ImportRequestStatusEnum.Created,
                    ApprovedByUserId = null,
                    CreationDate = SeedDate,
                },
                new ImportRequest
                {
                    Id = 2,
                    RequestDate = new DateOnly(2026, 3, 3),
                    Status = ImportRequestStatusEnum.Created,
                    ApprovedByUserId = 2,
                    CreationDate = SeedDate,
                },
                new ImportRequest
                {
                    Id = 3,
                    RequestDate = new DateOnly(2026, 3, 3),
                    Status = ImportRequestStatusEnum.Created,
                    ApprovedByUserId = 3,
                    CreationDate = SeedDate,
                },
                new ImportRequest
                {
                    Id = 4,
                    RequestDate = new DateOnly(2026, 3, 8),
                    Status = ImportRequestStatusEnum.Pending,
                    ApprovedByUserId = null,
                    CreationDate = SeedDate.AddDays(5),
                }
            );

            return modelBuilder;
        }
    }
}
