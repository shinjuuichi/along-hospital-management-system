using AutoMapper;
using MessageBroker.Events.SendEmailEvents;
using RecruitmentSvc.BLL.DTOs.InterviewDTOs;
using RecruitmentSvc.BLL.DTOs.JobApplicationDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.BLL.StateMachines;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;

namespace RecruitmentSvc.BLL.Implements
{
    public class JobApplicationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJobPostingService jobPostingService,
        IUploadFileService uploadFileService,
        IMessageBus messageBus)
        : BaseService<JobApplication, UpsertJobApplicationDTO, UpsertJobApplicationDTO, GetJobApplicationDTO>(
            unitOfWork, mapper, uploadFileService, includes: [nameof(JobApplication.JobPosting)]),
          IJobApplicationService
    {
        private readonly IGenericRepository<JobPosting> _jobPostingRepository = unitOfWork.Repository<JobPosting>();
        private readonly IJobPostingService _jobPostingService = jobPostingService;
        private readonly IMessageBus _messageBus = messageBus;

        #region Create
        public override async Task<GetJobApplicationDTO> CreateAsync(UpsertJobApplicationDTO createDTO)
        {
            string? cvUrl = null;

            try
            {
                var jobPostingId = createDTO.JobPostingId;
                var jobPosting = await _jobPostingService.GetByIdAsync(jobPostingId);

                if (Enum.TryParse<JobPostingStatusEnum>(jobPosting.Status, out var status)
                    && status == JobPostingStatusEnum.Closed)
                {
                    throw new InvalidDataException("Cannot apply to a closed JobPosting.");
                }

                if (jobPosting.CloseDate.HasValue && jobPosting.CloseDate.Value < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    throw new InvalidDataException("Cannot apply after the closing date.");
                }

                var existingApplication = await _repository.AnyAsync(
                    a => a.JobPostingId == jobPostingId && a.Email == createDTO.Email);

                if (existingApplication)
                {
                    throw new InvalidDataException("You have already applied to this JobPosting.");
                }

                if (createDTO.CVFile is { Length: > 0 })
                {
                    cvUrl = await _uploadFileService!.UploadAsync(createDTO.CVFile, nameof(JobApplication));
                    createDTO.CVUrl = cvUrl;
                }

                return await base.CreateAsync(createDTO);
            }
            catch
            {
                if (!string.IsNullOrEmpty(cvUrl))
                {
                    await _uploadFileService!.DeleteAsync(cvUrl);
                }

                throw;
            }
        }
        #endregion

        #region ChangeStatus
        public async Task UpdateStatusAsync(int id, JobApplicationStatusEnum newStatus)
        {
            var jobApplication = await _repository.GetByIdAsync(id,
                [nameof(JobApplication.Interviews), nameof(JobApplication.JobPosting)])
                ?? throw new DataNotFoundException(typeof(JobApplication), id);

            if (jobApplication.Interviews.Count == 0)
            {
                throw new InvalidDataException("Must have at least 1 interview before changing status.");
            }

            if (newStatus == JobApplicationStatusEnum.Passed)
            {
                var allPassed = jobApplication.Interviews.All(i => i.Result == InterviewResultEnum.Passed);
                if (!allPassed)
                {
                    throw new InvalidDataException("All interviews must be Passed before setting application status to Passed.");
                }
            }

            var jobApplicationStateMachine = new JobApplicationStatusStateMachine(jobApplication);

            if (!jobApplicationStateMachine.CanFire(newStatus))
            {
                throw new InvalidDataException($"Cannot transition application from '{jobApplication.ApplicationStatus}' to '{newStatus}'");
            }

            jobApplicationStateMachine.Fire(newStatus);
            _repository.Update(jobApplication);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion

        #region RejectAll
        public async Task RejectAllAppliedApplicationsAsync(int jobPostingId)
        {
            var jobPosting = await _jobPostingRepository.GetByIdAsync(jobPostingId,
                [nameof(JobPosting.JobApplications)])
                ?? throw new DataNotFoundException(typeof(JobPosting), jobPostingId);

            var notPassApplications = jobPosting.JobApplications
                .Where(a => a.ApplicationStatus == JobApplicationStatusEnum.Applied)
                .ToList();

            if (notPassApplications.Count == 0)
            {
                return;
            }

            foreach (var application in notPassApplications)
            {
                var jobApplicationStateMachine = new JobApplicationStatusStateMachine(application);

                if (!jobApplicationStateMachine.CanFire(JobApplicationStatusEnum.Failed))
                {
                    throw new InvalidDataException($"Cannot transition application from '{application.ApplicationStatus}' to '{JobApplicationStatusEnum.Failed}'");
                }

                jobApplicationStateMachine.Fire(JobApplicationStatusEnum.Failed);
            }

            _repository.UpdateRange(notPassApplications);
            await _unitOfWork.SaveChangeAsync();

            foreach (var application in notPassApplications)
            {
                await SendApplicationFailedEmailAsync(application, jobPosting);
            }
        }
        #endregion

        #region SendMail
        private async Task SendApplicationFailedEmailAsync(JobApplication jobApplication, JobPosting jobPosting)
        {
            var emailData = new GetInterviewMailDTO
            {
                CandidateName = jobApplication.Name,
                Email = jobApplication.Email,
                Phone = jobApplication.Phone,
                ApplyDate = jobApplication.ApplyDate,
                JobTitle = jobPosting.Title,
                Result = "Failed",
                ApplicationStatus = nameof(JobApplicationStatusEnum.Failed)
            };
            var emailEvent = _mapper.Map<SendInterviewResultEmailEvent>(emailData);
            await _messageBus.PublishAsync(emailEvent);
        }
        #endregion
    }
}