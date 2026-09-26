using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSvc.BLL.DTOs.FilterDTOs;
using RecruitmentSvc.BLL.DTOs.JobPostingDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace RecruitmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class JobPostingManagementController(IJobPostingService _jobPostingService)
        : CrudController<UpsertJobPostingDTO, UpsertJobPostingDTO, GetJobPostingDTO, JobPostingFilterDTO>(_jobPostingService)
    {
        protected override string? EntityName => "JobPosting";

        [HttpPut("open/{id:int}")]
        public async Task<IActionResult> Open(int id)
        {
            await _jobPostingService.UpdateStatusAsync(id, JobPostingStatusEnum.Open);
            return Result.SuccessAction("JobPosting status updated to Open successfully.");
        }

        [HttpPut("close/{id:int}")]
        public async Task<IActionResult> Close(int id)
        {
            await _jobPostingService.UpdateStatusAsync(id, JobPostingStatusEnum.Closed);
            return Result.SuccessAction("JobPosting status updated to Closed successfully.");
        }

        [AllowAnonymous]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }
    }
}