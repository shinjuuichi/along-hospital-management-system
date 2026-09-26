using MassTransit;
using MessageBroker.Contracts.RecruitmentContracts;
using MessageBroker.Events.RecruitmentEvents;
using RecruitmentSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace RecruitmentSvc.WebAPI.Consumers
{
    public class GetRecruitmentStatisticsByDateRangeConsumer(IRecruitmentStatisticsService recruitmentStatisticsService)
        : RequestConsumer<GetRecruitmentStatisticsByDateRangeEvent, GetRecruitmentStatisticsByDateRangeContract>
    {
        private readonly IRecruitmentStatisticsService _recruitmentStatisticsService = recruitmentStatisticsService;

        protected override async Task<GetRecruitmentStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetRecruitmentStatisticsByDateRangeEvent> context)
        {
            return await _recruitmentStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
