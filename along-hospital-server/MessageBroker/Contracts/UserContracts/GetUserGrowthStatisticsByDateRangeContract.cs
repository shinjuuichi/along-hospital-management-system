using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.UserContracts
{
    public record GetUserGrowthStatisticsByDateRangeContract : BaseContract
    {
        public int NewPatients { get; init; }
        public int NewStaff { get; init; }
        public StatisticsChartContract NewUsersOverTime { get; init; } = new();
        public StatisticsDistributionContract NewStaffRoleDistribution { get; init; } = new();
    }
}
