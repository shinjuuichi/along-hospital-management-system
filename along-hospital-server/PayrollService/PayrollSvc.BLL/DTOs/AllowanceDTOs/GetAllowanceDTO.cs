using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.AllowanceDTOs
{
    public class GetAllowanceDTO : MapFrom<Allowance>
    {
        #region Primary properties
        public int Id { get; set; }

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