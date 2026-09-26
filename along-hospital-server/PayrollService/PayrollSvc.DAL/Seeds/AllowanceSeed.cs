using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class AllowanceSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var allowances = new List<Allowance>
            {
                new Allowance { Id = 1, PayrollId = 1, AllowanceTypeId = 1, AllowanceTypeName = "Overtime", UnitPrice = 5, Quantity = 1, PayrollPolicyId = 2, IsTaxable = true }
            };

            var nextId = 2;
            for (var payrollId = 3; payrollId <= 40; payrollId++)
            {
                allowances.Add(new Allowance
                {
                    Id = nextId,
                    PayrollId = payrollId,
                    AllowanceTypeId = 2,
                    AllowanceTypeName = "Phu cap an trua",
                    UnitPrice = 14,
                    Quantity = 1,
                    PayrollPolicyId = null,
                    IsTaxable = false,
                    Note = "Meal allowance"
                });
                nextId++;

                if (payrollId % 3 == 0)
                {
                    allowances.Add(new Allowance
                    {
                        Id = nextId,
                        PayrollId = payrollId,
                        AllowanceTypeId = 1,
                        AllowanceTypeName = "Overtime",
                        UnitPrice = 7,
                        Quantity = 1,
                        PayrollPolicyId = null,
                        IsTaxable = true,
                        Note = "Overtime support"
                    });
                    nextId++;
                }
            }

            modelBuilder.Entity<Allowance>().HasData(allowances.ToArray());
            return modelBuilder;
        }
    }
}
