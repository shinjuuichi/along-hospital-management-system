using MessageBroker.Contracts.SupplierContracts;

namespace SupplierSvc.BLL.Interfaces
{
    public interface IImportStatisticsService
    {
        Task<GetImportStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate);
    }
}
