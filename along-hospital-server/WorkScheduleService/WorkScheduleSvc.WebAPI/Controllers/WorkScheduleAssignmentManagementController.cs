using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class WorkScheduleAssignmentManagementController(IWorkScheduleAssignmentService workScheduleAssignmentService)
        : CrudController<CreateWorkScheduleAssignmentDTO, UpdateWorkScheduleAssignmentDTO, GetWorkScheduleAssignmentDTO>(workScheduleAssignmentService)
    {
        private readonly IWorkScheduleAssignmentService _workScheduleAssignmentService = workScheduleAssignmentService;

        protected override string? EntityName => "WorkScheduleAssignment";

        public override async Task<IActionResult> Create(CreateWorkScheduleAssignmentDTO dto)
        {
            var result = await _workScheduleAssignmentService.BulkCreateAsync(dto);
            return Result.SuccessData(result);
        }
    }
}