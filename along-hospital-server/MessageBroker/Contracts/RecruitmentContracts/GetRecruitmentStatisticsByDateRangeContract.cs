using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.RecruitmentContracts
{
    public record GetRecruitmentStatisticsByDateRangeContract : BaseContract
    {
        public int DraftJobPostings { get; init; }
        public int OpenJobPostings { get; init; }
        public int ClosedJobPostings { get; init; }
        public int ApplicationsReceived { get; init; }
        public int InterviewingApplications { get; init; }
        public int PassedApplications { get; init; }
        public int FailedApplications { get; init; }
        public StatisticsDistributionContract JobPostingStatus { get; init; } = new();
        public StatisticsDistributionContract JobApplicationStatus { get; init; } = new();
        public StatisticsChartContract ApplicationsOverTime { get; init; } = new();
    }
}
