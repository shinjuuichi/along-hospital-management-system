using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDayShiftDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class WorkScheduleTemplateDayShiftManagementController(IWorkScheduleTemplateDayShiftService dayShiftService)
        : BaseController
    {
        private readonly IWorkScheduleTemplateDayShiftService _dayShiftService = dayShiftService;

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetDayShifts(int templateId)
        {
            var result = await _dayShiftService.GetDayShiftsAsync(templateId);
            return Result.SuccessData(result);
        }

        [HttpPut("{templateId}")]
        public async Task<IActionResult> UpdateDayShifts(int templateId, List<UpdateWorkScheduleTemplateDayShiftDTO> dayShifts)
        {
            await _dayShiftService.UpdateDayShiftsAsync(templateId, dayShifts);
            return Result.SuccessAction("Template day shifts updated successfully");
        }
    }
}
