using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Events.StaffRequestEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalStaffRequestStatisticsService(IMessageBus messageBus) : IExternalStaffRequestStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetStaffRequestStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetStaffRequestStatisticsByDateRangeEvent, GetStaffRequestStatisticsByDateRangeContract>(
                new GetStaffRequestStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
