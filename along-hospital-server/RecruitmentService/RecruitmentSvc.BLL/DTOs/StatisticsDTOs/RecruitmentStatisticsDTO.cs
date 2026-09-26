using MessageBroker.Contracts.StatisticsContracts;

namespace RecruitmentSvc.BLL.DTOs.StatisticsDTOs
{
    public class RecruitmentStatisticsDTO
    {
        public int DraftJobPostings { get; set; }
        public int OpenJobPostings { get; set; }
        public int ClosedJobPostings { get; set; }
        public int ApplicationsReceived { get; set; }
        public int InterviewingApplications { get; set; }
        public int PassedApplications { get; set; }
        public int FailedApplications { get; set; }
        public StatisticsDistributionContract JobPostingStatus { get; set; } = new();
        public StatisticsDistributionContract JobApplicationStatus { get; set; } = new();
        public StatisticsChartContract ApplicationsOverTime { get; set; } = new();
    }
}
