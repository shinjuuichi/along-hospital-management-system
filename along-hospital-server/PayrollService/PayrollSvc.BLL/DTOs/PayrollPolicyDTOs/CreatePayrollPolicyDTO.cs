using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.PayrollPolicyDTOs
{
    public class CreatePayrollPolicyDTO : MapTo<PayrollPolicy>
    {
        #region Primary properties
        public string? Name { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
        #endregion

        #region Foreign keys
        public List<int> StaffIds { get; set; } = [];

        public int? AllowanceTypeId { get; set; }

        public int? DeductionTypeId { get; set; }
        #endregion
    }
}