using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.TaxBracketDTOs
{
    public class GetTaxBracketDTO : MapFrom<TaxBracket>
    {
        #region Primary properties
        public int Id { get; set; }

        public double FromAmount { get; set; }

        public double TaxRate { get; set; }
        #endregion
    }
}
