using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AttendanceContracts
{
    public record AttendanceLogContract
    {
        public int StaffId { get; init; }

        public DateTime LogTime { get; init; }

        public string? LogType { get; init; }
    }

    public record GetAttendanceByStaffsAndRangeContract : BaseContract
    {
        public List<AttendanceLogContract> Data { get; init; } = [];
    }
}