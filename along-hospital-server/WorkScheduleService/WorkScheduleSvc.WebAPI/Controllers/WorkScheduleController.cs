using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.StaffRolePolicy)]
    public class WorkScheduleController(IWorkScheduleService workScheduleService) : BaseController
    {
        private readonly IWorkScheduleService _workScheduleService = workScheduleService;

        [HttpGet("staff")]
        public async Task<IActionResult> GetWorkScheduleForStaff([FromQuery] GetWorkScheduleRangeDTO rangeDTO)
        {
            var result = await _workScheduleService.GetWorkScheduleForStaffAsync(rangeDTO);
            return Result.SuccessData(result);
        }

        [HttpGet("all/doctor")]
        [Authorize(Roles = nameof(RoleEnum.Nurse))]
        public async Task<IActionResult> GetAllCurrentWorkingDoctors([FromQuery] int? specialtyId)
        {
            var result = await _workScheduleService.GetAllCurrentWorkingDoctorsAsync(specialtyId);
            return Result.SuccessData(result);
        }
    }
}
