using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.WorkScheduleContracts
{
    public record GetWorkScheduleContract : BaseContract
    {
        public int Id { get; init; }

        public DateOnly WorkDate { get; init; }

        public int ShiftId { get; init; }

        public GetShiftContract? Shift { get; init; }

        public List<GetWorkScheduleAssignmentContract> WorkScheduleAssignments { get; init; } = [];
    }

    public record GetListWorkScheduleContract : BaseContract
    {
        public List<GetWorkScheduleContract> Data { get; init; } = [];
    }
}