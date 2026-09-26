using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers
{
    public class StaffController(IStaffService staffService) : BaseController
    {
        private readonly IStaffService _staffService = staffService;

        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetStaffsByRoleAsync(string role, [FromQuery] FilterDTO filterDTO)
        {
            var staffs = await _staffService.GetStaffsByRoleAsync(role, filterDTO);
            return Result.SuccessData(staffs);
        }
    }
}
