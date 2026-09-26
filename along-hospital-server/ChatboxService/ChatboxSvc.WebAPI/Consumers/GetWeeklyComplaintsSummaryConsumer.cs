using ChatboxSvc.WebAPI.Interfaces;
using MassTransit;
using MessageBroker.Contracts.LLMContracts;
using MessageBroker.Events.LLMEvents;
using SharedLibrary.Base.MessageBuses;

namespace ChatboxSvc.WebAPI.Consumers
{
    public class GetWeeklyComplaintsSummaryConsumer(IAnalyticService analyticService)
        : RequestConsumer<GetWeeklyComplaintsSummaryEvent, GetWeeklyComplaintsSummaryContract>
    {
        private readonly IAnalyticService _analyticService = analyticService;

        protected override async Task<GetWeeklyComplaintsSummaryContract> Handle(ConsumeContext<GetWeeklyComplaintsSummaryEvent> context)
        {
            var complaints = context.Message.Complaints;
            var summary = await _analyticService.SummarizeWeeklyComplaintsAsync(complaints);

            return new GetWeeklyComplaintsSummaryContract
            {
                Summary = summary
            };
        }
    }
}