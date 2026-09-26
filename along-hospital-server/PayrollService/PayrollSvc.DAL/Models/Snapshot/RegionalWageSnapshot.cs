using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models.Snapshot
{
    public class RegionalWageSnapshot : Entity
    {
        public double MonthlyWage { get; set; }

        public int Region { get; set; }
    }
}