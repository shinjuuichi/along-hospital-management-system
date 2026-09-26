using AutoMapper;
using MessageBroker.Contracts.RecruitmentContracts;
using RecruitmentSvc.BLL.DTOs.StatisticsDTOs;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;

namespace RecruitmentSvc.BLL.Implements
{
    public class RecruitmentStatisticsService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRecruitmentStatisticsService
    {
        private readonly IGenericRepository<JobPosting> _jobPostingRepository = unitOfWork.Repository<JobPosting>();
        private readonly IGenericRepository<JobApplication> _jobApplicationRepository = unitOfWork.Repository<JobApplication>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetRecruitmentStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromUtc = DashboardStatisticsUtil.ToUtcStart(fromDate);
            var toExclusive = DashboardStatisticsUtil.ToUtcEndExclusive(toDate);

            var jobPostings = await _jobPostingRepository.GetAllAsync(
                jobPosting =>
                    jobPosting.CreationDate >= fromUtc &&
                    jobPosting.CreationDate < toExclusive);
            var jobApplications = await _jobApplicationRepository.GetAllAsync(
                jobApplication =>
                    jobApplication.ApplyDate >= fromDate &&
                    jobApplication.ApplyDate <= toDate);

            var applicationsByDay = jobApplications
                .GroupBy(jobApplication => jobApplication.ApplyDate)
                .ToDictionary(group => group.Key, group => group.Count());

            var statistics = new RecruitmentStatisticsDTO
            {
                DraftJobPostings = jobPostings.Count(jobPosting => jobPosting.Status == JobPostingStatusEnum.Draft),
                OpenJobPostings = jobPostings.Count(jobPosting => jobPosting.Status == JobPostingStatusEnum.Open),
                ClosedJobPostings = jobPostings.Count(jobPosting => jobPosting.Status == JobPostingStatusEnum.Closed),
                ApplicationsReceived = jobApplications.Count,
                InterviewingApplications = jobApplications.Count(jobApplication => jobApplication.ApplicationStatus == JobApplicationStatusEnum.Interviewing),
                PassedApplications = jobApplications.Count(jobApplication => jobApplication.ApplicationStatus == JobApplicationStatusEnum.Passed),
                FailedApplications = jobApplications.Count(jobApplication => jobApplication.ApplicationStatus == JobApplicationStatusEnum.Failed),
                JobPostingStatus = StatisticsContractBuilder.CreateDistribution(
                    jobPostings.GroupBy(jobPosting => jobPosting.Status)
                        .ToDictionary(group => group.Key, group => group.Count())),
                JobApplicationStatus = StatisticsContractBuilder.CreateDistribution(
                    jobApplications.GroupBy(jobApplication => jobApplication.ApplicationStatus)
                        .ToDictionary(group => group.Key, group => group.Count())),
                ApplicationsOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(fromDate, toDate, "applications", applicationsByDay)
            };

            return _mapper.Map<GetRecruitmentStatisticsByDateRangeContract>(statistics);
        }
    }
}
