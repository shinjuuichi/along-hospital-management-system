using RecruitmentSvc.BLL.DTOs.FilterDTOs;
using RecruitmentSvc.BLL.DTOs.JobPostingDTOs;
using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Results;

namespace RecruitmentSvc.BLL.Interfaces
{
    public interface IJobPostingService : IBaseCrudService<UpsertJobPostingDTO, UpsertJobPostingDTO, GetJobPostingDTO>
    {
        Task UpdateStatusAsync(int id, JobPostingStatusEnum newStatus);
        Task CloseExpiredJobPostingsAsync();
        Task<PaginationResult<GetJobPostingDTO>> GetAllActivePaginatedAsync(JobPostingFilterDTO filterDTO);
    }
}