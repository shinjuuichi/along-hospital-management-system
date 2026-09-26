using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalUserGrowthStatisticsService(IMessageBus messageBus) : IExternalUserGrowthStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetUserGrowthStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetUserGrowthStatisticsByDateRangeEvent, GetUserGrowthStatisticsByDateRangeContract>(
                new GetUserGrowthStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
