using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class PayrollPolicyStaffSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var payrollPolicyStaffs = new List<PayrollPolicyStaff>();
            var staffIds = new List<int> { 4, 2, 5, 6, 7, 8, 9, 11, 12, 13 };
            staffIds.AddRange(Enumerable.Range(14, 9));

            foreach (var staffId in staffIds.Distinct().OrderBy(x => x))
            {
                payrollPolicyStaffs.Add(new PayrollPolicyStaff
                {
                    PayrollPolicyId = 1,
                    StaffId = staffId
                });
            }

            payrollPolicyStaffs.Add(new PayrollPolicyStaff
            {
                PayrollPolicyId = 2,
                StaffId = 4
            });

            modelBuilder.Entity<PayrollPolicyStaff>().HasData(payrollPolicyStaffs.ToArray());
            return modelBuilder;
        }
    }
}
