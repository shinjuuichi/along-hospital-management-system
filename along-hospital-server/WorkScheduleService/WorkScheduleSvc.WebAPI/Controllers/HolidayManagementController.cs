using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.HolidayDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class HolidayManagementController(IHolidayService holidayService)
         : CrudController<UpsertHolidayDTO, UpsertHolidayDTO, GetHolidayDTO>(holidayService)
    {
        protected override string? EntityName => "Holiday";

        [HttpPost("range")]
        public async Task<IActionResult> CreateRange(CreateHolidayRangeDTO dto)
        {
            var result = await holidayService.CreateRangeAsync(dto);
            return Result.SuccessData(result, $"{result.Count} {EntityName}(s) created successfully");
        }
    }
}