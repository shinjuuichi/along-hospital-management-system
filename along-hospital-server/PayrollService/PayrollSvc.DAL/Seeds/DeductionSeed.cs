using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class DeductionSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var deductions = new List<Deduction>
            {
                new Deduction { Id = 1, PayrollId = 1, DeductionTypeId = 1, DeductionTypeName = "Late Penalty", UnitPrice = 1, Quantity = 2, Note = "Late 10 mins", PayrollPolicyId = 1, IsTaxable = false }
            };

            var nextId = 2;
            for (var payrollId = 3; payrollId <= 40; payrollId++)
            {
                deductions.Add(new Deduction
                {
                    Id = nextId,
                    PayrollId = payrollId,
                    DeductionTypeId = 3,
                    DeductionTypeName = "Phi cong doan",
                    UnitPrice = 2,
                    Quantity = 1,
                    Note = "Monthly union fee",
                    PayrollPolicyId = 1,
                    IsTaxable = false
                });
                nextId++;

                if (payrollId % 4 == 0)
                {
                    deductions.Add(new Deduction
                    {
                        Id = nextId,
                        PayrollId = payrollId,
                        DeductionTypeId = 1,
                        DeductionTypeName = "Late Penalty",
                        UnitPrice = 1,
                        Quantity = 1,
                        Note = "Late 5 mins",
                        PayrollPolicyId = null,
                        IsTaxable = false
                    });
                    nextId++;
                }
            }

            modelBuilder.Entity<Deduction>().HasData(deductions.ToArray());
            return modelBuilder;
        }
    }
}
