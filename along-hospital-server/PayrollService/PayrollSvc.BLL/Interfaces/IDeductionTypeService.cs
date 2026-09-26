using PayrollSvc.BLL.DTOs.DeductionTypeDTOs;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IDeductionTypeService : IBaseCrudService<CreateDeductionTypeDTO, UpdateDeductionTypeDTO, GetDeductionTypeDTO>
    {
        Task<List<Deduction>> BuildDeductionsAsync(List<PayrollPolicy> resolvedPolicies, CreatePayrollDTO createPayrollDTO);
    }
}
