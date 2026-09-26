using Microsoft.AspNetCore.Authorization;
using PayrollSvc.BLL.DTOs.PayrollPolicyDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class PayrollPolicyManagementController(IPayrollPolicyService payrollPolicyService)
    : CrudController<CreatePayrollPolicyDTO, UpdatePayrollPolicyDTO, GetPayrollPolicyDTO>(payrollPolicyService)
    {
        protected override string? EntityName => "PayrollPolicy";
    }
}
