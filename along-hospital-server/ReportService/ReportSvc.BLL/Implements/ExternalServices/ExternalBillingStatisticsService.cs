using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Events.BillingEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalBillingStatisticsService(IMessageBus messageBus) : IExternalBillingStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetBillingStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetBillingStatisticsByDateRangeEvent, GetBillingStatisticsByDateRangeContract>(
                new GetBillingStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
