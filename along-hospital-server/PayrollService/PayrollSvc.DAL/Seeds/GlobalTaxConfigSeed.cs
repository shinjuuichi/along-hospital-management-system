using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class GlobalTaxConfigSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<GlobalTaxConfig>().HasData(
                new GlobalTaxConfig
                {
                    Id = 1,
                    PersonalDeductionAmount = 440,
                    DependentDeductionAmount = 176,
                    ReferenceBaseSalary = 94,
                    SocialInsuranceRate = 0.08,
                    HealthInsuranceRate = 0.015,
                    UnemploymentInsuranceRate = 0.01,
                    CreationDate = seedDate
                }
            );
            return modelBuilder;
        }
    }
}
