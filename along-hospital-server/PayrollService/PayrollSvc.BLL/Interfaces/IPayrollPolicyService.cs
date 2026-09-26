using PayrollSvc.BLL.DTOs.PayrollPolicyDTOs;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IPayrollPolicyService : IBaseCrudService<CreatePayrollPolicyDTO, UpdatePayrollPolicyDTO, GetPayrollPolicyDTO>
    {
        Task<List<PayrollPolicy>> GetResolvedPoliciesByStaffAsync(int staffId, DateOnly payrollDate);
    }
}
