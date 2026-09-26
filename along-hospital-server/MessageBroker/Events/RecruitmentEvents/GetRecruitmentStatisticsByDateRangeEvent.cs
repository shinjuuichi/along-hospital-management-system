using MessageBroker.Abstractions;

namespace MessageBroker.Events.RecruitmentEvents
{
    public record GetRecruitmentStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
