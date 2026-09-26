using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.TeleSessionContracts
{
    public record GetListTeleSessionByAppointmentIdsContract : BaseContract
    {
        public List<GetTeleSessionByAppointmentIdContract> TeleSessions { get; init; } = [];
    }

    public record GetTeleSessionByAppointmentIdContract : BaseContract
    {
        public int AppointmentId { get; init; }

        public DateOnly Date { get; init; }

        public TimeOnly StartTime { get; init; }

        public TimeOnly EndTime { get; init; }

        public string? PatientJoinUrl { get; init; }
    }
}
