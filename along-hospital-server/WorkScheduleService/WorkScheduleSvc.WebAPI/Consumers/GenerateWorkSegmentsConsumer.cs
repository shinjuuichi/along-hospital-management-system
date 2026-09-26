using MassTransit;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class GenerateWorkSegmentsConsumer(IWorkSegmentService workSegmentService)
        : EventConsumer<GenerateWorkSegmentsEvent>
    {
        private readonly IWorkSegmentService _workSegmentService = workSegmentService;

        protected override async Task Handle(ConsumeContext<GenerateWorkSegmentsEvent> context)
        {
            await _workSegmentService.GenerateAsync(context.Message.PeriodStart, context.Message.PeriodEnd);
        }
    }
}