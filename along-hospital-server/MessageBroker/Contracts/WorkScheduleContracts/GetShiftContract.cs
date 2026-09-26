using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.WorkScheduleContracts
{
    public record GetShiftContract : BaseContract
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public TimeOnly StartTime { get; init; }

        public TimeOnly EndTime { get; init; }
    }

    public record GetListShiftsDataByIdsContract : BaseContract
    {
        public List<GetShiftContract> Data { get; init; } = [];
    }
}