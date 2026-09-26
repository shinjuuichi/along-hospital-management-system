using MessageBroker.Abstractions;

namespace MessageBroker.Events.VoucherEvents
{
    public record PreviewVoucherEvent : BaseEvent
    {
        public int PatientId { get; init; }

        public string? VoucherCode { get; init; }

        public List<MedicineDetailEventItem> Medicines { get; init; } = [];
    }
}