using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data.SqlServerDb;

namespace PayrollSvc.DAL.Data
{
    public class PayrollDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Deduction> Deduction { get; set; }
        public DbSet<DeductionType> DeductionType { get; set; }
        public DbSet<Allowance> Allowance { get; set; }
        public DbSet<AllowanceType> AllowanceType { get; set; }
        public DbSet<Payroll> Payroll { get; set; }
        public DbSet<PayrollPolicy> PayrollPolicy { get; set; }
        public DbSet<PayrollPolicyStaff> PayrollPolicyStaff { get; set; }
        public DbSet<TaxBracket> TaxBracket { get; set; }
        public DbSet<GlobalTaxConfig> GlobalTaxConfig { get; set; }
    }
}
