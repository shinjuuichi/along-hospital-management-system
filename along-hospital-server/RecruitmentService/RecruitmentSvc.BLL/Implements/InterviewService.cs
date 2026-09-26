using AutoMapper;
using MessageBroker.Events.SendEmailEvents;
using RecruitmentSvc.BLL.DTOs.InterviewDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.BLL.StateMachines;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace RecruitmentSvc.BLL.Implements
{
    public class InterviewService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        IJobApplicationService jobApplicationService)
        : BaseService<Interview, CreateInterviewDTO, UpdateInterviewDTO, GetInterviewDTO>(
            unitOfWork, mapper, includes: [nameof(Interview.JobApplication), nameof(Interview.InterviewType), "JobApplication.JobPosting"]),
          IInterviewService
    {
        private readonly IGenericRepository<JobApplication> _jobApplicationRepository = unitOfWork.Repository<JobApplication>();
        private readonly IGenericRepository<InterviewType> _interviewTypeRepository = unitOfWork.Repository<InterviewType>();
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IJobApplicationService _jobApplicationService = jobApplicationService;

        public override async Task<GetInterviewDTO> CreateAsync(CreateInterviewDTO createDTO)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(
                createDTO.JobApplicationId,
                [nameof(JobApplication.Interviews), nameof(JobApplication.JobPosting)])
                ?? throw new DataNotFoundException(typeof(JobApplication), createDTO.JobApplicationId);

            if (jobApplication.ApplicationStatus is JobApplicationStatusEnum.Passed or JobApplicationStatusEnum.Failed)
            {
                throw new InvalidDataException($"Cannot create interview for a JobApplication with status '{jobApplication.ApplicationStatus}'.");
            }

            var interviewType = await _interviewTypeRepository.GetByIdAsync(createDTO.InterviewTypeId)
                ?? throw new DataNotFoundException(typeof(InterviewType), createDTO.InterviewTypeId);

            if (createDTO.InterviewDate.HasValue)
            {
                var latestDate = jobApplication.Interviews.MaxBy(i => i.InterviewDate)?.InterviewDate;
                if (latestDate.HasValue)
                {
                    var newDate = DateOnly.FromDateTime(createDTO.InterviewDate.Value);
                    var existingDate = DateOnly.FromDateTime(latestDate.Value);
                    if (newDate <= existingDate)
                    {
                        throw new InvalidDataException(
                            $"New interview date ({createDTO.InterviewDate.Value:yyyy-MM-dd}) must be after " +
                            $"the latest existing interview date ({existingDate:yyyy-MM-dd}).");
                    }
                }
            }

            var result = await base.CreateAsync(createDTO);

            var newInterview = jobApplication.Interviews.First(i => i.Id == result.Id);

            var existingInterviews = jobApplication.Interviews
                .Where(i => i.Id != newInterview.Id)
                .ToList();

            var isFirstInterview = existingInterviews.Count == 0;
            var allPriorPassed = existingInterviews.All(i => i.Result == InterviewResultEnum.Passed);

            if (isFirstInterview || allPriorPassed)
            {
                await SendInterviewEmailAsync(newInterview, jobApplication);
            }

            if (jobApplication.ApplicationStatus == JobApplicationStatusEnum.Applied)
            {
                await _jobApplicationService.UpdateStatusAsync(jobApplication.Id, JobApplicationStatusEnum.Interviewing);
            }

            return result;
        }

        public override async Task<GetInterviewDTO> UpdateAsync(int id, UpdateInterviewDTO updateDTO)
        {
            var interview = await _repository.GetByIdAsync(id,
                [
                    nameof(Interview.JobApplication),
                    nameof(Interview.InterviewType),
                    $"{nameof(Interview.JobApplication)}.{nameof(JobApplication.JobPosting)}",
                    $"{nameof(Interview.JobApplication)}.{nameof(JobApplication.Interviews)}"
                ])
                ?? throw new DataNotFoundException(typeof(Interview), id);

            var jobApplication = interview.JobApplication
                ?? throw new DataNotFoundException(typeof(JobApplication), interview.JobApplicationId);

            if (jobApplication.ApplicationStatus is JobApplicationStatusEnum.Passed or JobApplicationStatusEnum.Failed)
            {
                throw new InvalidDataException($"Cannot update interview for a JobApplication with status '{jobApplication.ApplicationStatus}'.");
            }

            var hasResultChange = !string.IsNullOrWhiteSpace(updateDTO.Result);
            InterviewResultEnum newResult = InterviewResultEnum.Pending;
            if (hasResultChange &&
                !Enum.TryParse<InterviewResultEnum>(updateDTO.Result, true, out newResult))
            {
                throw new InvalidDataException($"Invalid interview result '{updateDTO.Result}'.");
            }
            var isDateChanging = updateDTO.InterviewDate.HasValue;
            var isInterviewTypeChanging = updateDTO.InterviewTypeId != interview.InterviewTypeId;
            var isNoteChanging = !string.IsNullOrWhiteSpace(updateDTO.Note);
            var hasInfoChange = isDateChanging || isInterviewTypeChanging || isNoteChanging;

            if (hasResultChange && hasInfoChange)
            {
                throw new InvalidDataException("Cannot change interview info and result in the same request.");
            }

            if (hasInfoChange)
            {
                await HandleInfoChangeAsync(interview, jobApplication, updateDTO, isDateChanging, isInterviewTypeChanging);
                return _mapper.Map<GetInterviewDTO>(interview);
            }

            if (hasResultChange)
            {
                await HandleResultChangeAsync(interview, jobApplication, newResult!);
                return _mapper.Map<GetInterviewDTO>(interview);
            }

            _mapper.Map(updateDTO, interview);
            _repository.Update(interview);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetInterviewDTO>(interview);
        }

        private async Task HandleInfoChangeAsync(
            Interview interview,
            JobApplication jobApplication,
            UpdateInterviewDTO updateDTO,
            bool isDateChanging,
            bool isInterviewTypeChanging)
        {
            if (isInterviewTypeChanging)
            {
                var interviewType = await _interviewTypeRepository.GetByIdAsync(updateDTO.InterviewTypeId)
                    ?? throw new DataNotFoundException(typeof(InterviewType), updateDTO.InterviewTypeId);
                interview.InterviewTypeId = interviewType.Id;
            }

            if (isDateChanging)
            {
                var currentDate = DateOnly.FromDateTime(interview.InterviewDate);
                var newDate = DateOnly.FromDateTime(updateDTO.InterviewDate!.Value);

                var prevInterview = jobApplication.Interviews
                    .Where(i => i.Id != interview.Id && DateOnly.FromDateTime(i.InterviewDate) < currentDate)
                    .OrderBy(i => i.InterviewDate)
                    .LastOrDefault();

                var nextInterview = jobApplication.Interviews
                    .Where(i => i.Id != interview.Id && DateOnly.FromDateTime(i.InterviewDate) > currentDate)
                    .OrderBy(i => i.InterviewDate)
                    .FirstOrDefault();

                if (prevInterview != null && newDate <= DateOnly.FromDateTime(prevInterview.InterviewDate))
                {
                    throw new InvalidDataException(
                        $"Rescheduled interview date ({updateDTO.InterviewDate!.Value:yyyy-MM-dd}) must be after " +
                        $"the previous interview date ({prevInterview.InterviewDate:yyyy-MM-dd}).");
                }

                if (nextInterview != null && newDate >= DateOnly.FromDateTime(nextInterview.InterviewDate))
                {
                    throw new InvalidDataException(
                        $"Rescheduled interview date ({updateDTO.InterviewDate!.Value:yyyy-MM-dd}) must be before " +
                        $"the next interview date ({nextInterview.InterviewDate:yyyy-MM-dd}).");
                }

                interview.InterviewDate = updateDTO.InterviewDate!.Value;
            }

            if (!string.IsNullOrWhiteSpace(updateDTO.Note))
            {
                interview.Note = updateDTO.Note;
            }

            _repository.Update(interview);
            await _unitOfWork.SaveChangeAsync();

            await SendInterviewEmailAsync(interview, jobApplication);
        }

        private async Task HandleResultChangeAsync(Interview interview, JobApplication jobApplication, InterviewResultEnum newResult)
        {
            var stateMachine = new InterviewResultStateMachine(interview);
            if (!stateMachine.CanFire(newResult))
            {
                throw new InvalidDataException($"Cannot transition interview result from '{interview.Result}' to '{newResult}'.");
            }

            if (newResult is InterviewResultEnum.Passed or InterviewResultEnum.Failed &&
                DateTime.UtcNow < interview.InterviewDate)
            {
                throw new InvalidDataException("Cannot change interview result before the interview date.");
            }

            var allInterviews = jobApplication.Interviews.OrderBy(i => i.InterviewDate).ToList();
            var earliestPending = allInterviews.FirstOrDefault(i => i.Result == InterviewResultEnum.Pending);
            if (earliestPending != null && earliestPending.Id != interview.Id)
            {
                throw new InvalidDataException(
                    $"Cannot change interview result before completing the earliest pending interview " +
                    $"(InterviewDate: {earliestPending.InterviewDate:yyyy-MM-dd}).");
            }

            interview.Result = newResult;
            _repository.Update(interview);
            await _unitOfWork.SaveChangeAsync();

            switch (newResult)
            {
                case InterviewResultEnum.Passed:
                    await HandlePassedResultAsync(interview, jobApplication);
                    break;

                case InterviewResultEnum.Failed:
                case InterviewResultEnum.Cancelled:
                    await HandleFailedOrCancelledResultAsync(interview, jobApplication);
                    break;
            }
        }

        private async Task HandlePassedResultAsync(Interview interview, JobApplication jobApplication)
        {
            await SendInterviewResultEmailAsync(interview, jobApplication);

            var allInterviews = jobApplication.Interviews.ToList();
            var nextInterview = FindNextInterview(interview, allInterviews);

            if (nextInterview != null)
            {
                var nextWithDetails = await _repository.GetByIdAsync(
                    nextInterview.Id,
                    [nameof(Interview.InterviewType), $"{nameof(Interview.JobApplication)}.{nameof(JobApplication.JobPosting)}"]);

                if (nextWithDetails != null)
                {
                    await SendInterviewEmailAsync(nextWithDetails, jobApplication);
                }
            }
        }

        private async Task HandleFailedOrCancelledResultAsync(Interview interview, JobApplication jobApplication)
        {
            await SendInterviewResultEmailAsync(interview, jobApplication);

            var pendingInterviews = jobApplication.Interviews
                .Where(i => i.Id != interview.Id && i.Result == InterviewResultEnum.Pending)
                .ToList();

            foreach (var pending in pendingInterviews)
            {
                pending.Result = InterviewResultEnum.Failed;
            }
            _repository.UpdateRange(pendingInterviews);
            await _unitOfWork.SaveChangeAsync();

            await _jobApplicationService.UpdateStatusAsync(jobApplication.Id, JobApplicationStatusEnum.Failed);
        }

        private async Task SendInterviewEmailAsync(Interview interview, JobApplication jobApplication)
        {
            var emailData = _mapper.Map<GetInterviewMailDTO>(jobApplication);
            _mapper.Map(interview, emailData);
            var interviewEmailEvent = _mapper.Map<SendInterviewEmailEvent>(emailData);
            await _messageBus.PublishAsync(interviewEmailEvent);
        }

        private async Task SendInterviewResultEmailAsync(Interview interview, JobApplication jobApplication)
        {
            var emailData = _mapper.Map<GetInterviewMailDTO>(jobApplication);
            _mapper.Map(interview, emailData);
            emailData.ApplicationStatus = interview.Result.ToString();
            var interviewResultEmailEvent = _mapper.Map<SendInterviewResultEmailEvent>(emailData);
            await _messageBus.PublishAsync(interviewResultEmailEvent);
        }

        public async Task<List<GetInterviewDTO>> GetAllByJobApplicationIdAsync(int jobApplicationId)
        {
            var interviews = await _repository.GetAllAsync(
                filter: i => i.JobApplicationId == jobApplicationId,
                includes: _includes);

            var sortedInterviews = interviews.OrderBy(i => i.InterviewDate).ToList();
            return _mapper.Map<List<GetInterviewDTO>>(sortedInterviews);
        }

        private Interview? FindNextInterview(Interview current, IEnumerable<Interview> allInterviews)
        {
            return allInterviews
                .Where(i => i.Id != current.Id && i.InterviewDate > current.InterviewDate)
                .OrderBy(i => i.InterviewDate)
                .FirstOrDefault();
        }
    }
}
