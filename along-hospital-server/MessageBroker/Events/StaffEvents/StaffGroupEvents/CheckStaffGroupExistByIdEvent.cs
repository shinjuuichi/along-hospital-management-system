using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents.StaffGroupEvents
{
    public record CheckStaffGroupExistByIdEvent : BaseEvent
    {
        public int StaffGroupId { get; init; }
    }

    public record CheckStaffGroupExistByIdContract : BaseContract;
}