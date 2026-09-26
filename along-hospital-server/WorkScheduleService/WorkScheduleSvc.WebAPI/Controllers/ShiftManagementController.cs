using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.ShiftDTOs;
using WorkScheduleSvc.BLL.FilterDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class ShiftManagementController(IShiftService shiftService)
        : CrudController<CreateShiftDTO, UpdateShiftDTO, GetShiftDTO, ShiftFilterDTO>(shiftService)
    {
        protected override string? EntityName => "Shift";
    }
}
