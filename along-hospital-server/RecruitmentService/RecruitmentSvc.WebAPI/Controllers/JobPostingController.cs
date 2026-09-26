using Microsoft.AspNetCore.Mvc;
using RecruitmentSvc.BLL.DTOs.FilterDTOs;
using RecruitmentSvc.BLL.DTOs.JobPostingDTOs;
using RecruitmentSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;

namespace RecruitmentSvc.WebAPI.Controllers
{
    public class JobPostingController(IJobPostingService _jobPostingService)
       : GetController<GetJobPostingDTO, JobPostingFilterDTO>(_jobPostingService)
    {
        [HttpGet]
        public override async Task<IActionResult> GetAllPaginated(JobPostingFilterDTO filterDTO)
        {
            var result = await _jobPostingService.GetAllActivePaginatedAsync(filterDTO);
            return Result.SuccessData(result);
        }
    }
}