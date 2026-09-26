using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSvc.BLL.DTOs.DeductionTypeDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class DeductionTypeManagementController(IDeductionTypeService deductionTypeService)
        : CrudController<CreateDeductionTypeDTO, UpdateDeductionTypeDTO, GetDeductionTypeDTO>(deductionTypeService)
    {
        protected override string EntityName => "Deduction Type";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}
