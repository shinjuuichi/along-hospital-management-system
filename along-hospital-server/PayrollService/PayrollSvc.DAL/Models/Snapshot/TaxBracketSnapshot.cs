using SharedLibrary.Commons.EntityAbstractions;

namespace PayrollSvc.DAL.Models.Snapshot
{
    public class TaxBracketSnapshot : Entity
    {
        public double FromAmount { get; set; }

        public double TaxRate { get; set; }
    }
}
