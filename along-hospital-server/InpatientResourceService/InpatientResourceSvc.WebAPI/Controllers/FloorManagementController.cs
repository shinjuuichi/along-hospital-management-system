using InpatientResourceSvc.BLL.DTOs.FloorDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class FloorManagementController(IFloorService floorService)
        : CrudController<UpsertFloorDTO, UpsertFloorDTO, GetFloorDTO>(floorService)
    {
        protected override string? EntityName => "Floor";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}