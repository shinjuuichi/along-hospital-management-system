using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace PayrollSvc.DAL.Models
{
    public class Payroll : AuditEntity
    {
        #region Primary properties
        [MessageRange(1, 12)]
        public int Month { get; set; } = DateTime.UtcNow.Month;

        [MessageRange(1900, 2100)]
        public int Year { get; set; } = DateTime.UtcNow.Year;

        [NumberHigherThanOrEqualTo(0)]
        public int TotalWorkedMinutes { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public int OvertimeMinutes { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public int LateMinutes { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public int EarlyLeaveMinutes { get; set; }

        public PayrollStatusEnum Status { get; set; } = PayrollStatusEnum.Draft;

        public Guid? TransactionId { get; set; }

        public string? QrCode { get; set; }

        public int StaffId { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<Allowance> Allowances { get; set; } = [];
        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<Deduction> Deductions { get; set; } = [];
        #endregion

        #region Snapshot properties
        [JsonColumn]
        public virtual GlobalTaxConfigSnapshot? GlobalTaxConfigSnapshot { get; set; }

        [JsonColumn]
        public virtual RegionalWageSnapshot? RegionalWageSnapshot { get; set; }

        [JsonColumn]
        public virtual StaffContractSnapshot? StaffContractSnapshot { get; set; }

        [JsonColumn]
        public virtual StaffSnapshot? StaffSnapshot { get; set; }

        [JsonColumn]
        public virtual SalaryAdvanceSnapshot? SalaryAdvanceSnapshot { get; set; }

        [JsonColumn]
        public virtual List<TaxBracketSnapshot> TaxBracketSnapshot { get; set; } = [];
        #endregion

        #region Computed properties
        public double TotalAllowance => Allowances.Sum(a => a.UnitPrice * a.Quantity);

        public double TotalDeduction => Deductions.Sum(d => d.UnitPrice * d.Quantity);

        public double SalaryAdvanceAmount => SalaryAdvanceSnapshot?.Amount ?? 0;

        public double BaseSalary => TotalWorkedMinutes / 60.0 * StaffContractSnapshot!.HourlyRate;

        public double GrossSalary => BaseSalary + TotalAllowance;

        public double NetSalary => GrossSalary - TotalDeduction - TotalInsurance - PersonalIncomeTax - SalaryAdvanceAmount;

        public double SocialInsurance => GlobalTaxConfigSnapshot!.SocialInsuranceRate * SocialInsuranceSalaryCapped;

        public double HealthInsurance => GlobalTaxConfigSnapshot!.HealthInsuranceRate * HealthInsuranceSalaryCapped;

        public double UnemploymentInsurance => GlobalTaxConfigSnapshot!.UnemploymentInsuranceRate * UnemploymentInsuranceSalaryCapped;

        public double TotalInsurance => SocialInsurance + HealthInsurance + UnemploymentInsurance;

        public double TotalAllowanceNotTaxable => Allowances.Where(a => !a.IsTaxable).Sum(a => a.UnitPrice * a.Quantity);

        public double TaxableIncome => GrossSalary - TotalAllowanceNotTaxable;

        public double AssessableIncome => TaxableIncome - TotalInsurance - GlobalTaxConfigSnapshot!.PersonalDeductionAmount
            - (StaffSnapshot!.DependentQuantity * GlobalTaxConfigSnapshot.DependentDeductionAmount);

        public double PersonalIncomeTax
        {
            get
            {
                var brackets = TaxBracketSnapshot?
                    .OrderBy(b => b.FromAmount)
                    .ToList() ?? [];

                if (brackets.Count == 0 || AssessableIncome <= 0)
                {
                    return 0;
                }

                double total = 0;
                for (int i = 0; i < brackets.Count; i++)
                {
                    var current = brackets[i];
                    var nextFrom = i + 1 < brackets.Count ? brackets[i + 1].FromAmount : (double?)null;

                    if (AssessableIncome <= current.FromAmount)
                    {
                        break;
                    }

                    double upperBound = nextFrom.HasValue ? Math.Min(AssessableIncome, nextFrom.Value) : AssessableIncome;
                    double taxable = Math.Max(0, upperBound - current.FromAmount);
                    total += taxable * current.TaxRate;
                }

                return total;
            }
        }
        #endregion

        #region Helper methods
        private double SocialInsuranceSalaryCapped => Math.Min(
        InsuranceSalaryBase,
        GlobalTaxConfigSnapshot!.ReferenceBaseSalary * 20);

        private double HealthInsuranceSalaryCapped => Math.Min(
        InsuranceSalaryBase,
        GlobalTaxConfigSnapshot!.ReferenceBaseSalary * 20);

        private double UnemploymentInsuranceSalaryCapped => Math.Min(
        InsuranceSalaryBase,
        RegionalWageSnapshot!.MonthlyWage * 20);

        private double InsuranceSalaryBase => BaseSalary * StaffContractSnapshot!.InsuranceSalaryRate;
        #endregion
    }
}
