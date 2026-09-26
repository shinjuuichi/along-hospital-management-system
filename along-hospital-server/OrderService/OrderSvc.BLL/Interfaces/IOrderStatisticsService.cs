using MessageBroker.Contracts.OrderContracts;

namespace OrderSvc.BLL.Interfaces
{
    public interface IOrderStatisticsService
    {
        Task<GetOrderStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
        Task<GetTopSellingMedicinesByDateRangeContract> GetTopSellingMedicinesByDateRangeAsync(DateOnly fromDate, DateOnly toDate, int topN);
    }
}