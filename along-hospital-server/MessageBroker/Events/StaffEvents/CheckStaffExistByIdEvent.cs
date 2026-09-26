using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents
{
    public record CheckStaffExistByIdEvent : BaseEvent
    {
        public int StaffId { get; init; }
    }

    public record CheckStaffExistByIdsEvent : BaseEvent
    {
        public List<int> StaffIds { get; init; } = [];
    }

    public record CheckStaffExistByIdContract : BaseContract;
    public record CheckStaffExistByIdsContract : BaseContract;
}
