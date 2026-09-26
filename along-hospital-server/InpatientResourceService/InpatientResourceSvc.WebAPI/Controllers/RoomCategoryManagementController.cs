using InpatientResourceSvc.BLL.DTOs.RoomCategoryDTOs;
using InpatientResourceSvc.BLL.FilterDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class RoomCategoryManagementController(IRoomCategoryService roomCategoryService)
        : CrudController<UpsertRoomCategoryDTO, UpsertRoomCategoryDTO, GetRoomCategoryDTO, RoomCategoryFilterDTO>(roomCategoryService)
    {
        protected override string? EntityName => "Room Category";
    }
}