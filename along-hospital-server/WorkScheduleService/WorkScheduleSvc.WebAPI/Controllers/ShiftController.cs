using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Settings;
using WorkScheduleSvc.BLL.DTOs.ShiftDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.StaffRolePolicy)]
    public class ShiftController(IShiftService shiftService) : GetController<GetShiftDTO>(shiftService);
}
