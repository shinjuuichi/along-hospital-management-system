using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models.Snapshot
{
    public class SalaryAdvanceSnapshot : Entity
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public string? Reason { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

        public int? ModifiedBy { get; set; }
    }
}