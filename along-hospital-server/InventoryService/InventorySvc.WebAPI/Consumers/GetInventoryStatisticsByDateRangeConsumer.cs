using InventorySvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace InventorySvc.WebAPI.Consumers
{
    public class GetInventoryStatisticsByDateRangeConsumer(IInventoryStatisticsService inventoryStatisticsService)
        : RequestConsumer<GetInventoryStatisticsByDateRangeEvent, GetInventoryStatisticsByDateRangeContract>
    {
        private readonly IInventoryStatisticsService _inventoryStatisticsService = inventoryStatisticsService;

        protected override async Task<GetInventoryStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetInventoryStatisticsByDateRangeEvent> context)
        {
            return await _inventoryStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
