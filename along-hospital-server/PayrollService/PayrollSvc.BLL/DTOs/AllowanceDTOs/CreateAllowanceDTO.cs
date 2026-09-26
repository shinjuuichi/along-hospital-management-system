using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.AllowanceDTOs
{
    public class CreateAllowanceDTO : MapTo<Allowance>
    {
        #region Primary properties
        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }

        public bool IsTaxable { get; set; }
        #endregion

        #region Foreign keys
        public int PayrollId { get; set; }

        public int AllowanceTypeId { get; set; }

        public string? AllowanceTypeName { get; set; }

        public int? PayrollPolicyId { get; set; }
        #endregion
    }
}