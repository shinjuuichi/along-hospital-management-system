using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class StaffContractSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var futureEndDate = new DateOnly(2030, 12, 31);

            modelBuilder.Entity<StaffContract>().HasData(
                new StaffContract
                {
                    Id = 2,
                    ContractCode = "HD002",
                    ContractType = StaffContractTypeEnum.Probation,
                    StartDate = new DateOnly(2024, 6, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 2.2,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 6, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 2,
                    RegionalWageId = 2,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 4,
                    ContractCode = "HD004",
                    ContractType = StaffContractTypeEnum.FixedTerm,
                    StartDate = new DateOnly(2024, 3, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 3,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 3, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 4,
                    RegionalWageId = 1,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 5,
                    ContractCode = "HD005",
                    ContractType = StaffContractTypeEnum.Internship,
                    StartDate = new DateOnly(2024, 9, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 1.4,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 9, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 5,
                    RegionalWageId = 3,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 6,
                    ContractCode = "HD006",
                    ContractType = StaffContractTypeEnum.Indefinite,
                    StartDate = new DateOnly(2023, 5, 1),
                    EndDate = null,
                    HourlyRate = 4,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2023, 5, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 6,
                    RegionalWageId = 1,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 7,
                    ContractCode = "HD007",
                    ContractType = StaffContractTypeEnum.Probation,
                    StartDate = new DateOnly(2024, 10, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 2,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 10, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 7,
                    RegionalWageId = 2,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 8,
                    ContractCode = "HD008",
                    ContractType = StaffContractTypeEnum.FixedTerm,
                    StartDate = new DateOnly(2024, 2, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 3.2,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 2, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 8,
                    RegionalWageId = 1,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 9,
                    ContractCode = "HD009",
                    ContractType = StaffContractTypeEnum.Indefinite,
                    StartDate = new DateOnly(2023, 8, 15),
                    EndDate = null,
                    HourlyRate = 3.8,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2023, 8, 15),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 9,
                    RegionalWageId = 2,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 11,
                    ContractCode = "HD011",
                    ContractType = StaffContractTypeEnum.FixedTerm,
                    StartDate = new DateOnly(2024, 4, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 2.8,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 4, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 11,
                    RegionalWageId = 1,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 12,
                    ContractCode = "HD012",
                    ContractType = StaffContractTypeEnum.Probation,
                    StartDate = new DateOnly(2024, 11, 1),
                    EndDate = futureEndDate,
                    HourlyRate = 2.08,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2024, 11, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 12,
                    RegionalWageId = 2,
                    CreationDate = seedCreationDate
                },
                new StaffContract
                {
                    Id = 13,
                    ContractCode = "HD013",
                    ContractType = StaffContractTypeEnum.Indefinite,
                    StartDate = new DateOnly(2023, 12, 1),
                    EndDate = null,
                    HourlyRate = 3.6,
                    WorkingHoursPerWeek = 40,
                    Status = StaffContractStatusEnum.Active,
                    SignedDate = new DateOnly(2023, 12, 1),
                    SignatureImage = null,
                    InsuranceSalaryRate = 1,
                    StaffId = 13,
                    RegionalWageId = 1,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
