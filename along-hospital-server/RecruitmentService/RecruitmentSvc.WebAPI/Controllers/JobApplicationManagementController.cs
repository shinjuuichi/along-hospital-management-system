using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSvc.BLL.DTOs.FilterDTOs;
using RecruitmentSvc.BLL.DTOs.JobApplicationDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace RecruitmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class JobApplicationManagementController(IJobApplicationService _jobApplicationService)
        : CrudController<UpsertJobApplicationDTO, UpsertJobApplicationDTO, GetJobApplicationDTO, FilterJobApplicationDTO>(_jobApplicationService)
    {
        protected override string? EntityName => "JobApplication";

        [AllowAnonymous]
        public override Task<IActionResult> Create(UpsertJobApplicationDTO createDTO)
        {
            return base.Create(createDTO);
        }

        [HttpPut("pass/{jobApplicationId}")]
        public async Task<IActionResult> Pass(int jobApplicationId)
        {
            await _jobApplicationService.UpdateStatusAsync(jobApplicationId, JobApplicationStatusEnum.Passed);
            return Result.SuccessAction("Application status updated to Passed successfully.");
        }

        [HttpPut("fail/{jobApplicationId}")]
        public async Task<IActionResult> Fail(int jobApplicationId)
        {
            await _jobApplicationService.UpdateStatusAsync(jobApplicationId, JobApplicationStatusEnum.Failed);
            return Result.SuccessAction("Application status updated to Failed successfully.");
        }

        [HttpPut("reject-all/{jobPostingId}")]
        public async Task<IActionResult> RejectAllApplied(int jobPostingId)
        {
            await _jobApplicationService.RejectAllAppliedApplicationsAsync(jobPostingId);
            return Result.SuccessAction("All applied applications have been rejected successfully.");
        }
    }
}