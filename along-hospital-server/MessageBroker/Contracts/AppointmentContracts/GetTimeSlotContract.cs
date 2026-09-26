using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AppointmentContracts
{
    public record GetTimeSlotContract : BaseContract
    {
        public int Id { get; init; }

        public TimeOnly Time { get; init; }

        public int CapacityPerDoctor { get; init; }
    }

    public record GetAllTimeSlotsContract : BaseContract
    {
        public List<GetTimeSlotContract> Data { get; init; } = [];
    }
}