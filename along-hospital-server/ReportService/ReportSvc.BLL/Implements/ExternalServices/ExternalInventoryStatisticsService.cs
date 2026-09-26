using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalInventoryStatisticsService(IMessageBus messageBus) : IExternalInventoryStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetInventoryStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetInventoryStatisticsByDateRangeEvent, GetInventoryStatisticsByDateRangeContract>(
                new GetInventoryStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
