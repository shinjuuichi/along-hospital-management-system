using MessageBroker.Contracts.RecruitmentContracts;
using MessageBroker.Events.RecruitmentEvents;
using ReportSvc.BLL.FilterDTOs;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Base.MessageBuses;

namespace ReportSvc.BLL.Implements.ExternalServices
{
    public class ExternalRecruitmentStatisticsService(IMessageBus messageBus) : IExternalRecruitmentStatisticsService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetRecruitmentStatisticsByDateRangeContract> GetStatisticsAsync(DashboardDateRangeFilterDTO filterDTO)
        {
            return await _messageBus.RequestAsync<GetRecruitmentStatisticsByDateRangeEvent, GetRecruitmentStatisticsByDateRangeContract>(
                new GetRecruitmentStatisticsByDateRangeEvent { FromDate = filterDTO.FromDate, ToDate = filterDTO.ToDate });
        }
    }
}
