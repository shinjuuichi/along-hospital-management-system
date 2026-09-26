using MessageBroker.Contracts.StatisticsContracts;

namespace UserSvc.BLL.DTOs.StatisticsDTOs
{
    public class UserGrowthStatisticsDTO
    {
        public int NewPatients { get; set; }
        public int NewStaff { get; set; }
        public StatisticsChartContract NewUsersOverTime { get; set; } = new();
        public StatisticsDistributionContract NewStaffRoleDistribution { get; set; } = new();
    }
}
