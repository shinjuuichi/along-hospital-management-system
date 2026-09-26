using InpatientResourceSvc.BLL.DTOs.BedCategoryDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class BedCategoryManagementController(IBedCategoryService bedCategoryService)
        : CrudController<CreateBedCategoryDTO, UpdateBedCategoryDTO, GetBedCategoryDTO>(bedCategoryService)
    {
        protected override string? EntityName => "Bed Category";

        [AllowAnonymous]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }
    }
}