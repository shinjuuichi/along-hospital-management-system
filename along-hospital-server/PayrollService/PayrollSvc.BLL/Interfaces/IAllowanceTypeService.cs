using PayrollSvc.BLL.DTOs.AllowanceTypeDTOs;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IAllowanceTypeService : IBaseCrudService<CreateAllowanceTypeDTO, UpdateAllowanceTypeDTO, GetAllowanceTypeDTO>
    {
        Task<List<Allowance>> BuildAllowancesAsync(List<PayrollPolicy> resolvedPolicies, CreatePayrollDTO createPayrollDTO);
    }
}