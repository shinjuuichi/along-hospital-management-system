using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace StaffSvc.DAL.Models
{
    public class RegionalWage : AuditEntity
    {
        [Unique, NumberHigherThanOrEqualTo(1)]
        public int Code { get; set; }

        [NumberHigherThan(0)]
        public double MonthlyWage { get; set; }
    }
}