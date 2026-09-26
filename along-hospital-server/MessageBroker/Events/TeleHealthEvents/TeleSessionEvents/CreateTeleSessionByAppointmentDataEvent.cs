using MessageBroker.Abstractions;

namespace MessageBroker.Events.TeleHealthEvents.TeleSessionEvents
{
    public record CreateTeleSessionByAppointmentDataEvent : BaseEvent
    {
        public int AppointmentId { get; init; }

        public int SpecialtyId { get; init; }

        public int PatientId { get; init; }

        public DateOnly Date { get; init; }

        public TimeOnly StartTime { get; init; }

        public TimeOnly EndTime { get; init; }
    }
}
