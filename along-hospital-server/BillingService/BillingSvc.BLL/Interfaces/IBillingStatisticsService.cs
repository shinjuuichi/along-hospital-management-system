using MessageBroker.Contracts.BillingContracts;

namespace BillingSvc.BLL.Interfaces
{
    public interface IBillingStatisticsService
    {
        Task<GetBillingStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
