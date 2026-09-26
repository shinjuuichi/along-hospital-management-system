using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.FilterDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class WorkScheduleManagementController(
        IWorkScheduleService workScheduleService)
        : CrudController<CreateWorkScheduleDTO, UpdateWorkScheduleDTO, GetWorkScheduleDTO, WorkScheduleFilterDTO>(workScheduleService)
    {
        private readonly IWorkScheduleService _workScheduleService = workScheduleService;

        protected override string EntityName => "WorkSchedule";

        public override async Task<IActionResult> Create(CreateWorkScheduleDTO createDTO)
        {
            var result = await _workScheduleService.GenerateAsync(createDTO);
            return Result.SuccessData(result, "Work schedules created successfully");
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetWorkScheduleForDateRange([FromQuery] GetWorkScheduleRangeDTO rangeDTO)
        {
            var result = await _workScheduleService.GetWorkScheduleForDateRangeAsync(rangeDTO);
            return Result.SuccessData(result);
        }

        [HttpPut("publish")]
        public async Task<IActionResult> Publish(UpdateWorkScheduleStatusRangeDTO dto)
        {
            var result = await _workScheduleService.UpdateStatusRangeAsync(dto, WorkScheduleStatusEnum.Published);
            return Result.SuccessData(result, "Work schedule published successfully");
        }

        [HttpPut("lock")]
        public async Task<IActionResult> Lock(UpdateWorkScheduleStatusRangeDTO dto)
        {
            var result = await _workScheduleService.UpdateStatusRangeAsync(dto, WorkScheduleStatusEnum.Locked);
            return Result.SuccessData(result, "Work schedule locked successfully");
        }

        [HttpPut("finalize")]
        public async Task<IActionResult> Finalize(UpdateWorkScheduleStatusRangeDTO dto)
        {
            var result = await _workScheduleService.FinalizeRangeAsync(dto);
            return Result.SuccessData(result, "Work schedule finalized successfully");
        }
    }
}
