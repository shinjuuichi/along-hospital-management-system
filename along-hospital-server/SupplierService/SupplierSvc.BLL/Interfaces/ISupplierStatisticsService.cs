using MessageBroker.Contracts.SupplierContracts;

namespace SupplierSvc.BLL.Interfaces
{
    public interface ISupplierStatisticsService
    {
        Task<GetSupplierStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
