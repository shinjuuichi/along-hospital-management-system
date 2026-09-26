using Microsoft.AspNetCore.Authorization;
using PayrollSvc.BLL.DTOs.AllowanceDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class AllowanceManagementController(IAllowanceService allowanceService)
        : CrudController<CreateAllowanceDTO, UpdateAllowanceDTO, GetAllowanceDTO>(allowanceService)
    {
        protected override string? EntityName => "Allowance";
    }
}
