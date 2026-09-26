using PayrollSvc.BLL.DTOs.AllowanceDTOs;
using PayrollSvc.BLL.DTOs.DeductionDTOs;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.PayrollDTOs
{
    public class UpdatePayrollDTO : MapTo<Payroll>
    {
        #region Primary properties
        public List<GetAllowanceDTO> Allowances { get; set; } = [];

        public List<GetDeductionDTO> Deductions { get; set; } = [];
        #endregion
    }
}