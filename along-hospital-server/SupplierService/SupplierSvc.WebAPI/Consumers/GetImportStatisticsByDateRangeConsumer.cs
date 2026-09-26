using MassTransit;
using SupplierSvc.BLL.Interfaces;
using MessageBroker.Contracts.SupplierContracts;
using MessageBroker.Events.SupplierEvents;
using SharedLibrary.Base.MessageBuses;

namespace SupplierSvc.WebAPI.Consumers
{
    public class GetImportStatisticsByDateRangeConsumer(IImportStatisticsService importStatisticsService)
        : RequestConsumer<GetImportStatisticsByDateRangeEvent, GetImportStatisticsByDateRangeContract>
    {
        private readonly IImportStatisticsService _importStatisticsService = importStatisticsService;

        protected override async Task<GetImportStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetImportStatisticsByDateRangeEvent> context)
        {
            return await _importStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
