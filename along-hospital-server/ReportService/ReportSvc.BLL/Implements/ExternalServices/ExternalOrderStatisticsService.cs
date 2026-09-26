using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Events.OrderEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalOrderStatisticsService(IMessageBus messageBus) : IExternalOrderStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetOrderStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetOrderStatisticsByDateRangeEvent, GetOrderStatisticsByDateRangeContract>(
                new GetOrderStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }

        public async Task<GetTopSellingMedicinesByDateRangeContract> GetTopSellingMedicinesAsync(DashboardDateRangeFilterDTO filterDTO, int topN = 5)
        {
            return await _messageBus.RequestAsync<GetTopSellingMedicinesByDateRangeEvent, GetTopSellingMedicinesByDateRangeContract>(
                new GetTopSellingMedicinesByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate, TopN = topN });
        }
    }
}
