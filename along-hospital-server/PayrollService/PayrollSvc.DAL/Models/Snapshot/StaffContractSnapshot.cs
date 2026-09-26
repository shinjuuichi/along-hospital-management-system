using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models.Snapshot
{
    public class StaffContractSnapshot : Entity
    {
        public double HourlyRate { get; set; }

        public double InsuranceSalaryRate { get; set; }
    }
}
