using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AppointmentContracts
{
    public record GetAppointmentContract : BaseContract
    {
        public int Id { get; init; }

        public DateOnly Date { get; init; }

        public TimeOnly Time { get; init; }

        public string? Purpose { get; init; }

        public string? AppointmentStatus { get; init; }

        public string? AppointmentMeetingType { get; init; }

        public string? AppointmentPaymentStatus { get; init; }

        public DateTime? CompletedDate { get; init; }

        public DateTime? CancelledDate { get; init; }

        public int? MedicalHistoryId { get; init; }

        public int SpecialtyId { get; init; }

        public int PatientId { get; init; }

        public int TimeSlotId { get; init; }
    }

    public record GetListAppointmentDataContract : BaseContract
    {
        public List<GetAppointmentContract> Data { get; init; } = [];
    }
}
