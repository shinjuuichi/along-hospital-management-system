using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalMedicalHistoryStatisticsService(IMessageBus messageBus) : IExternalMedicalHistoryStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetMedicalHistoryStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetMedicalHistoryStatisticsByDateRangeEvent, GetMedicalHistoryStatisticsByDateRangeContract>(
                new GetMedicalHistoryStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
