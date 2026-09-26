using MessageBroker.Contracts.InventoryContracts;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces.ExternalServices
{
    public interface IExternalInventoryStatisticsService
    {
        Task<GetInventoryStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
