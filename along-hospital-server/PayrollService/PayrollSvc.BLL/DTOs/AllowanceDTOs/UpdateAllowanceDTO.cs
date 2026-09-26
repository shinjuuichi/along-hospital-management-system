using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.AllowanceDTOs
{
    public class UpdateAllowanceDTO : MapTo<Allowance>
    {
        #region Primary properties
        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
        #endregion
    }
}