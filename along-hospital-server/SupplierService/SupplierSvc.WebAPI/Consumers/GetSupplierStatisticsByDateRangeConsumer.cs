using MassTransit;
using SupplierSvc.BLL.Interfaces;
using MessageBroker.Contracts.SupplierContracts;
using MessageBroker.Events.SupplierEvents;
using SharedLibrary.Base.MessageBuses;

namespace SupplierSvc.WebAPI.Consumers
{
    public class GetSupplierStatisticsByDateRangeConsumer(ISupplierStatisticsService supplierStatisticsService)
        : RequestConsumer<GetSupplierStatisticsByDateRangeEvent, GetSupplierStatisticsByDateRangeContract>
    {
        private readonly ISupplierStatisticsService _supplierStatisticsService = supplierStatisticsService;

        protected override async Task<GetSupplierStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetSupplierStatisticsByDateRangeEvent> context)
        {
            return await _supplierStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
