using InpatientResourceSvc.BLL.DTOs.RoomDTOs;
using InpatientResourceSvc.BLL.FilterDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class RoomManagementController(IRoomService _roomService)
        : CrudController<CreateRoomDTO, UpdateRoomDTO, GetRoomDTO, RoomFilterDTO>(_roomService)
    {
        protected override string? EntityName => "Room";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }

        [AllowAnonymous]
        [HttpGet("role")]
        public async Task<IActionResult> GetAllByRoles()
        {
            var rooms = await _roomService.GetAllByRolesAsync();
            return Result.SuccessData(rooms);
        }
    }
}