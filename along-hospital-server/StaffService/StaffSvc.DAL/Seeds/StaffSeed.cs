using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class StaffSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var specialties = Enumerable.Range(1, 9).ToArray();

            List<Staff> staffs =
            [
                new Staff
                {
                    Id = 2,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.Vietcombank,
                    AccountNumber = "9704360000000002",
                    DependentQuantity = 0,
                    QualificationId = 2,
                    SpecialtyId = 2,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 4,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.Techcombank,
                    AccountNumber = "9704070000000004",
                    DependentQuantity = 1,
                    QualificationId = 4,
                    SpecialtyId = 4,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 5,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.BIDV,
                    AccountNumber = "9704180000000005",
                    DependentQuantity = 0,
                    QualificationId = 5,
                    SpecialtyId = 5,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 6,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.VietinBank,
                    AccountNumber = "9704150000000006",
                    DependentQuantity = 2,
                    QualificationId = 6,
                    SpecialtyId = 6,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 7,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.ACB,
                    AccountNumber = "9704160000000007",
                    DependentQuantity = 1,
                    QualificationId = 7,
                    SpecialtyId = 7,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 8,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.TPBank,
                    AccountNumber = "9704230000000008",
                    DependentQuantity = 0,
                    QualificationId = 8,
                    SpecialtyId = 8,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 9,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.VPBank,
                    AccountNumber = "9704320000000009",
                    DependentQuantity = 0,
                    QualificationId = 9,
                    SpecialtyId = 9,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 11,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.Agribank,
                    AccountNumber = "9704050000000011",
                    DependentQuantity = 1,
                    QualificationId = 11,
                    SpecialtyId = 11,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 12,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.MSB,
                    AccountNumber = "9704260000000012",
                    DependentQuantity = 0,
                    QualificationId = 12,
                    SpecialtyId = 12,
                    CreationDate = seedCreationDate
                },
                new Staff
                {
                    Id = 13,
                    Status = StaffStatusEnum.Active,
                    BankCode = BankCodeEnum.OCB,
                    AccountNumber = "9704480000000013",
                    DependentQuantity = 2,
                    QualificationId = 13,
                    SpecialtyId = 13,
                    CreationDate = seedCreationDate
                }
            ];

            foreach (var specialtyId in specialties)
            {
                var specialtyIndex = specialtyId - 1;
                var doctorId = 13 + specialtyId;

                staffs.Add(new Staff
                {
                    Id = doctorId,
                    Status = StaffStatusEnum.Active,
                    BankCode = (BankCodeEnum)(specialtyIndex % 25),
                    AccountNumber = $"9704{specialtyId:00}{doctorId:0000000000}",
                    DependentQuantity = specialtyIndex % 3,
                    QualificationId = (specialtyIndex % 10) + 1,
                    SpecialtyId = specialtyId,
                    CreationDate = seedCreationDate
                });
            }

            modelBuilder.Entity<Staff>().HasData(staffs.ToArray());

            return modelBuilder;
        }
    }
}
