using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models.Snapshot
{
    public class StaffSnapshot : Entity
    {
        public int DependentQuantity { get; set; }

        public string? BankCode { get; set; }

        public string? AccountNumber { get; set; }
    }
}
