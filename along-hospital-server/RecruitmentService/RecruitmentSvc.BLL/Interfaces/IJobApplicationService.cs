using RecruitmentSvc.BLL.DTOs.JobApplicationDTOs;
using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Base.Services;
using System.Collections.Generic;

namespace RecruitmentSvc.BLL.Interfaces
{
    public interface IJobApplicationService : IBaseCrudService<UpsertJobApplicationDTO, UpsertJobApplicationDTO, GetJobApplicationDTO>
    {
        Task UpdateStatusAsync(int id, JobApplicationStatusEnum targetStatus);
        Task RejectAllAppliedApplicationsAsync(int jobPostingId);
    }
}