using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.SnapshotDTOs.TaxBracketSnapshotDTOs
{
    public class GetTaxBracketSnapshotDTO : MapFrom<TaxBracketSnapshot>
    {
        public double FromAmount { get; set; }

        public double TaxRate { get; set; }
    }
}
