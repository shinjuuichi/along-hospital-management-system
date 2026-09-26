using MassTransit;
using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Events.OrderEvents;
using OrderSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace OrderSvc.WebAPI.Consumers
{
    public class GetOrderStatisticsByDateRangeConsumer(IOrderStatisticsService orderStatisticsService)
        : RequestConsumer<GetOrderStatisticsByDateRangeEvent, GetOrderStatisticsByDateRangeContract>
    {
        private readonly IOrderStatisticsService _orderStatisticsService = orderStatisticsService;

        protected override async Task<GetOrderStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetOrderStatisticsByDateRangeEvent> context)
        {
            return await _orderStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
