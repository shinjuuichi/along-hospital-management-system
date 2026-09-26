using InpatientResourceSvc.BLL.DTOs.BuildingDTOs;
using InpatientResourceSvc.BLL.FilterDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class BuildingManagementController(IBuildingService buildingService)
        : CrudController<UpsertBuildingDTO, UpsertBuildingDTO, GetBuildingDTO, BuildingFilterDTO>(buildingService)
    {
        protected override string? EntityName => "Building";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}