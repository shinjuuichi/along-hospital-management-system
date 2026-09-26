using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using TeleHealthSvc.BLL.DTOs.TeleRoomDTOs;
using TeleHealthSvc.BLL.Interfaces;

namespace TeleHealthSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Doctor))]
    public class TeleRoomController(
        ITeleRoomService teleRoomService) : GetController<GetTeleRoomDTO>(teleRoomService)
    {
        private readonly ITeleRoomService _teleRoomService = teleRoomService;

        [HttpGet("room")]
        public async Task<IActionResult> GetTeleRoomForDoctorAsync()
        {
            var teleRoom = await _teleRoomService.GetTeleRoomForDoctorAsync();
            return Result.SuccessData(teleRoom);
        }

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}