using MessageBroker.Abstractions;

namespace MessageBroker.Events.VoucherEvents
{
    public record PreviewListMedicineDiscountEvent : BaseEvent
    {
        public List<PreviewMedicineDetailEvent> Medicines { get; init; } = [];
    }

    public record PreviewMedicineDetailEvent : BaseEvent
    {
        public int MedicineId { get; init; }

        public string? SKUCode { get; init; }

        public double Price { get; init; }
    }
}