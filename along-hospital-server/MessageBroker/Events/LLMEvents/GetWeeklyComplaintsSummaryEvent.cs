using MessageBroker.Abstractions;

namespace MessageBroker.Events.LLMEvents
{
    public record GetWeeklyComplaintsSummaryEvent : BaseEvent
    {
        public List<string> Complaints { get; init; } = [];
    }
}