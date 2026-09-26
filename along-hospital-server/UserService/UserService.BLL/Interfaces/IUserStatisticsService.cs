using MessageBroker.Contracts.UserContracts;

namespace UserSvc.BLL.Interfaces
{
    public interface IUserStatisticsService
    {
        Task<GetUserGrowthStatisticsByDateRangeContract> GetUserGrowthStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
