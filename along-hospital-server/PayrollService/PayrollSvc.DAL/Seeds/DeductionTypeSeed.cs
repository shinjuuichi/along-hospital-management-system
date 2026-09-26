using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class DeductionTypeSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<DeductionType>().HasData(
                new DeductionType
                {
                    Id = 1,
                    Name = "Late Penalty",
                    Price = 2000,
                    IsTaxable = false,
                    IsSystemGenerated = true,
                    IsPercentage = false,
                    InsuranceSubject = InsuranceSubjectEnum.None,
                    DeductionQuantitySourceEnum = DeductionQuantitySourceEnum.LateMinutes,
                    CreationDate = seedDate
                },
                new DeductionType
                {
                    Id = 2,
                    Name = "Early Leave Penalty",
                    Price = 2000,
                    IsTaxable = false,
                    IsSystemGenerated = true,
                    IsPercentage = false,
                    InsuranceSubject = InsuranceSubjectEnum.None,
                    DeductionQuantitySourceEnum = DeductionQuantitySourceEnum.EarlyLeaveMinutes,
                    CreationDate = seedDate
                },
                new DeductionType
                {
                    Id = 3,
                    Name = "Phi cong doan",
                    Price = 100000,
                    IsTaxable = false,
                    IsSystemGenerated = false,
                    IsPercentage = false,
                    InsuranceSubject = InsuranceSubjectEnum.None,
                    DeductionQuantitySourceEnum = DeductionQuantitySourceEnum.None,
                    CreationDate = seedDate
                }
            );
            return modelBuilder;
        }
    }
}
