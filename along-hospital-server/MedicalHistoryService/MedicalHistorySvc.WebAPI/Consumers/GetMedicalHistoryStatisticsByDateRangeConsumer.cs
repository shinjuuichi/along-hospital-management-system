using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class GetMedicalHistoryStatisticsByDateRangeConsumer(IMedicalHistoryStatisticsService medicalHistoryStatisticsService)
        : RequestConsumer<GetMedicalHistoryStatisticsByDateRangeEvent, GetMedicalHistoryStatisticsByDateRangeContract>
    {
        private readonly IMedicalHistoryStatisticsService _medicalHistoryStatisticsService = medicalHistoryStatisticsService;

        protected override async Task<GetMedicalHistoryStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetMedicalHistoryStatisticsByDateRangeEvent> context)
        {
            return await _medicalHistoryStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
