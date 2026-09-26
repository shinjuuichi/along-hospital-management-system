using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalAppointmentStatisticsService(IMessageBus messageBus) : IExternalAppointmentStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetAppointmentStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetAppointmentStatisticsByDateRangeEvent, GetAppointmentStatisticsByDateRangeContract>(
                new GetAppointmentStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
