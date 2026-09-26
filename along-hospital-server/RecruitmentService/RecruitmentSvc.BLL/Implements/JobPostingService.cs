using AutoMapper;
using RecruitmentSvc.BLL.DTOs.FilterDTOs;
using RecruitmentSvc.BLL.DTOs.JobPostingDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.BLL.StateMachines;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using System.Linq.Expressions;

namespace RecruitmentSvc.BLL.Implements
{
    public class JobPostingService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<JobPosting, UpsertJobPostingDTO, UpsertJobPostingDTO, GetJobPostingDTO>(unitOfWork, mapper, includes: [nameof(JobPosting.JobApplications)]),
          IJobPostingService
    {
        public override async Task<GetJobPostingDTO> UpdateAsync(int id, UpsertJobPostingDTO updateDTO)
        {
            var jobPosting = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(JobPosting), id);

            if (jobPosting.Status != JobPostingStatusEnum.Draft)
            {
                throw new InvalidDataException("JobPosting can only be updated when status is Draft.");
            }

            return await base.UpdateAsync(id, updateDTO);
        }

        public async Task UpdateStatusAsync(int id, JobPostingStatusEnum newStatus)
        {
            var jobPosting = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(JobPosting), id);

            var stateMachine = new JobPostingStatusStateMachine(jobPosting);

            if (!stateMachine.CanFire(newStatus))
            {
                throw new InvalidDataException($"Cannot transition JobPosting from '{jobPosting.Status}' to '{newStatus}'");
            }

            stateMachine.Fire(newStatus);
            _repository.Update(jobPosting);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task CloseExpiredJobPostingsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var expiredJobPostings = await _repository.GetAllAsync(
                jp => jp.CloseDate.HasValue &&
                      jp.CloseDate.Value < today &&
                      jp.Status == JobPostingStatusEnum.Open);

            foreach (var jobPosting in expiredJobPostings)
            {
                var stateMachine = new JobPostingStatusStateMachine(jobPosting);

                if (stateMachine.CanFire(JobPostingStatusEnum.Closed))
                {
                    stateMachine.Fire(JobPostingStatusEnum.Closed);
                }
            }

            if (expiredJobPostings.Any())
            {
                _repository.UpdateRange(expiredJobPostings);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task<PaginationResult<GetJobPostingDTO>> GetAllActivePaginatedAsync(JobPostingFilterDTO filterDTO)
        {
            Expression<Func<JobPosting, bool>> statusFilter = jp => jp.Status != JobPostingStatusEnum.Draft;

            var (total, entities) = await _repository.GetAllPaginatedAsync(
                statusFilter,
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            return new PaginationResult<GetJobPostingDTO>(total, filterDTO.PageSize, _mapper.Map<List<GetJobPostingDTO>>(entities));
        }
    }
}
