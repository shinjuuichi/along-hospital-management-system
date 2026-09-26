using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class PayrollPolicySeed : ISeedBuilder
    {
        public int Priority => 2;
        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<PayrollPolicy>().HasData(
                new PayrollPolicy
                {
                    Id = 1,
                    Name = "Standard Policy 2025",
                    StartDate = new DateOnly(2025, 1, 1),
                    EndDate = new DateOnly(2025, 12, 31),
                    PayrollPolicyStatus = PayrollPolicyStatus.Active,
                    CreationDate = seedDate,
                    DeductionTypeId = 3,
                },
                new PayrollPolicy
                {
                    Id = 2,
                    Name = "Policy An Trua 2026",
                    StartDate = new DateOnly(2026, 1, 1),
                    EndDate = new DateOnly(2026, 12, 31),
                    PayrollPolicyStatus = PayrollPolicyStatus.Active,
                    AllowanceTypeId = 2,
                    CreationDate = seedDate,
                }
            );
            return modelBuilder;
        }
    }
}
