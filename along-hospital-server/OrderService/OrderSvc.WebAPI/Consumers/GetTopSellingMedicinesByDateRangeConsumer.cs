using MassTransit;
using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Events.OrderEvents;
using OrderSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace OrderSvc.WebAPI.Consumers
{
    public class GetTopSellingMedicinesByDateRangeConsumer(IOrderStatisticsService orderStatisticsService)
        : RequestConsumer<GetTopSellingMedicinesByDateRangeEvent, GetTopSellingMedicinesByDateRangeContract>
    {
        private readonly IOrderStatisticsService _orderStatisticsService = orderStatisticsService;

        protected override async Task<GetTopSellingMedicinesByDateRangeContract> Handle(
            ConsumeContext<GetTopSellingMedicinesByDateRangeEvent> context)
        {
            return await _orderStatisticsService.GetTopSellingMedicinesByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate,
                context.Message.TopN);
        }
    }
}
