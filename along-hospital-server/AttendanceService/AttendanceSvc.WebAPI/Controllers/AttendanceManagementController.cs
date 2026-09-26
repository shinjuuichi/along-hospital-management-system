using AttendanceSvc.BLL.FilterDTOs;
using AttendanceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace AttendanceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class AttendanceManagementController(IAttendanceService attendanceService) : BaseController
    {
        private readonly IAttendanceService _attendanceService = attendanceService;

        [HttpGet]
        public async Task<IActionResult> GetAll(AttendanceFilterDTO attendanceFilterDTO)
        {
            var getAll = await _attendanceService.GetAllPaginatedAsync(attendanceFilterDTO);
            return Result.SuccessData(getAll);
        }
    }
}