using MessageBroker.Contracts.SupplierContracts;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces.ExternalServices
{
    public interface IExternalSupplierStatisticsService
    {
        Task<GetSupplierStatisticsByDateRangeContract> GetSupplierStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
        Task<GetImportStatisticsByDateRangeContract> GetImportStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
    }
}
