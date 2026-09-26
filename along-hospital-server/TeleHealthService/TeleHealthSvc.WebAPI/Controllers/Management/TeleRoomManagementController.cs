using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;
using TeleHealthSvc.BLL.DTOs.TeleRoomDTOs;
using TeleHealthSvc.BLL.FilterDTOs;
using TeleHealthSvc.BLL.Interfaces;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class TeleRoomManagementController(ITeleRoomService teleRoomService)
        : CrudController<CreateTeleRoomDTO, UpdateTeleRoomDTO, GetTeleRoomDTO, TeleRoomFilterDTO>(teleRoomService)
    {
        protected override string? EntityName => nameof(TeleRoom);
    }
}