using MessageBroker.Contracts.InventoryContracts;

namespace InventorySvc.BLL.Interfaces
{
    public interface IInventoryStatisticsService
    {
        Task<GetInventoryStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
