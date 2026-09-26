using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.TeleSessionContracts
{
    public record GetAppointmentByTeleSessionIdContract : BaseContract
    {
        public DateOnly Date { get; init; }

        public TimeOnly StartTime { get; init; }

        public TimeOnly EndTime { get; init; }

        public string? AppointmentPaymentStatus { get; init; }
    }
}
