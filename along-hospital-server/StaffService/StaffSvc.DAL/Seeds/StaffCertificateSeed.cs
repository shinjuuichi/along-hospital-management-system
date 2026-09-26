using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class StaffCertificateSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffCertificate>().HasData(
                new StaffCertificate
                {
                    Id = 2,
                    CertificateNo = "ACLS-002",
                    IssuedDate = new DateOnly(2023, 6, 10),
                    ExpiredDate = new DateOnly(2025, 6, 10),
                    IssuedBy = "American Heart Association",
                    Status = StaffCertificateStatusEnum.Valid,
                    StaffCertificateTypeId = 2,
                    StaffId = 4,
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null,
                    ModificationDate = null,
                    ModifiedBy = null,
                    DeletionDate = null,
                    IsDeleted = false
                },
                new StaffCertificate
                {
                    Id = 3,
                    CertificateNo = "PALS-003",
                    IssuedDate = new DateOnly(2022, 9, 5),
                    ExpiredDate = new DateOnly(2024, 9, 5),
                    IssuedBy = "American Heart Association",
                    Status = StaffCertificateStatusEnum.Expired,
                    StaffCertificateTypeId = 3,
                    StaffId = 5,
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null,
                    ModificationDate = null,
                    ModifiedBy = null,
                    DeletionDate = null,
                    IsDeleted = false
                }
            );

            return modelBuilder;
        }
    }
}
