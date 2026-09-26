using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSvc.BLL.DTOs.AllowanceTypeDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Accountant))]
    public class AllowanceTypeManagementController(IAllowanceTypeService allowanceTypeService)
        : CrudController<CreateAllowanceTypeDTO, UpdateAllowanceTypeDTO, GetAllowanceTypeDTO>(allowanceTypeService)
    {
        protected override string EntityName => "Allowance Type";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}
