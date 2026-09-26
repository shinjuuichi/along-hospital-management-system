using Microsoft.AspNetCore.Authorization;
using PayrollSvc.BLL.DTOs.DeductionDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class DeductionManagementController(IDeductionService deductionService)
        : CrudController<CreateDeductionDTO, UpdateDeductionDTO, GetDeductionDTO>(deductionService)
    {
        protected override string? EntityName => "Deduction";
    }
}
