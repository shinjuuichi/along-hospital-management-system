using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class AllowanceTypeSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<AllowanceType>().HasData(
                new AllowanceType
                {
                    Id = 1,
                    Name = "Overtime",
                    Price = 1000000,
                    IsTaxable = true,
                    IsSystemGenerated = true,
                    IsPercentage = false,
                    InsuranceSubject = InsuranceSubjectEnum.None,
                    AllowanceQuantitySourceEnum = AllowanceQuantitySourceEnum.OvertimeMinutes,
                    CreationDate = seedDate
                },
                new AllowanceType
                {
                    Id = 2,
                    Name = "Phu cap an trua",
                    Price = 500000,
                    IsTaxable = false,
                    IsSystemGenerated = false,
                    IsPercentage = false,
                    InsuranceSubject = InsuranceSubjectEnum.None,
                    AllowanceQuantitySourceEnum = AllowanceQuantitySourceEnum.None,
                    CreationDate = seedDate
                }
            );
            return modelBuilder;
        }
    }
}
