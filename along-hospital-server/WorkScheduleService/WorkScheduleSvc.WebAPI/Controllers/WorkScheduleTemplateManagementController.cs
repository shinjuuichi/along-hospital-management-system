using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class WorkScheduleTemplateManagementController(IWorkScheduleTemplateService workScheduleTemplateService)
        : CrudController<CreateWorkScheduleTemplateDTO, UpdateWorkScheduleTemplateDTO, GetWorkScheduleTemplateDTO>(workScheduleTemplateService)
    {
        private readonly IWorkScheduleTemplateService _workScheduleTemplateService = workScheduleTemplateService;

        protected override string? EntityName => "Work Schedule Template";

        [HttpPost("duplicate/{id}")]
        public async Task<IActionResult> Duplicate(int id)
        {
            await _workScheduleTemplateService.DuplicateAsync(id);
            return Result.SuccessAction("Work schedule template duplicated successfully");
        }
    }
}
