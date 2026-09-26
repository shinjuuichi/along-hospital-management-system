using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class GetBillingStatisticsByDateRangeConsumer(IBillingStatisticsService billingStatisticsService)
        : RequestConsumer<GetBillingStatisticsByDateRangeEvent, GetBillingStatisticsByDateRangeContract>
    {
        private readonly IBillingStatisticsService _billingStatisticsService = billingStatisticsService;

        protected override async Task<GetBillingStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetBillingStatisticsByDateRangeEvent> context)
        {
            return await _billingStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
