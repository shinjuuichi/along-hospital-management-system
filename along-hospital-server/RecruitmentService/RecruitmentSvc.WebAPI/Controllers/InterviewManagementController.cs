using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSvc.BLL.DTOs.InterviewDTOs;
using RecruitmentSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace RecruitmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class InterviewManagementController(IInterviewService _interviewService)
        : CrudController<CreateInterviewDTO, UpdateInterviewDTO, GetInterviewDTO>(_interviewService)
    {
        protected override string? EntityName => "Interview";

        [HttpGet("by-job-application/{jobApplicationId}")]
        public async Task<IActionResult> GetAllByJobApplicationId(int jobApplicationId)
        {
            var result = await _interviewService.GetAllByJobApplicationIdAsync(jobApplicationId);
            return Result.SuccessData(result);
        }
    }
}