using MessageBroker.Contracts.OrderContracts;
using ReportSvc.BLL.FilterDTOs;

namespace ReportSvc.BLL.Interfaces.ExternalServices
{
    public interface IExternalOrderStatisticsService
    {
        Task<GetOrderStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO);
        Task<GetTopSellingMedicinesByDateRangeContract> GetTopSellingMedicinesAsync(DashboardDateRangeFilterDTO filterDTO, int topN = 5);
    }
}
