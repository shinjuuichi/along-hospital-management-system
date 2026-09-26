using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetUserGrowthStatisticsByDateRangeConsumer(IUserStatisticsService userStatisticsService)
        : RequestConsumer<GetUserGrowthStatisticsByDateRangeEvent, GetUserGrowthStatisticsByDateRangeContract>
    {
        private readonly IUserStatisticsService _userStatisticsService = userStatisticsService;

        protected override async Task<GetUserGrowthStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetUserGrowthStatisticsByDateRangeEvent> context)
        {
            return await _userStatisticsService.GetUserGrowthStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
