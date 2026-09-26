using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.TaxBracketDTOs
{
    public class UpdateTaxBracketDTO : MapTo<TaxBracket>
    {
        #region Primary properties
        public double FromAmount { get; set; }

        public double TaxRate { get; set; }
        #endregion
    }
}
