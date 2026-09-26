using MessageBroker.Abstractions;
using MessageBroker.Contracts.WorkScheduleContracts;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GetAllWorkingShiftsEvent : BaseEvent;

    public record GetAllWorkingShiftsContract : BaseContract
    {
        public List<GetShiftContract> Data { get; init; } = [];
    }
}