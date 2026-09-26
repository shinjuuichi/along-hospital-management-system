using MessageBroker.Abstractions;

namespace MessageBroker.Events.OrderEvents
{
    public record CreateOrderByCartDataEvent : BaseEvent
    {
        public int UserId { get; init; }
        public string? VoucherCode { get; init; }
        public string? PaymentType { get; init; }
        public string? Description { get; init; }
        public bool IsPickupAtStore { get; init; }
        public List<CartMedicineEventItem> CartMedicineEventItems { get; init; } = [];
    }

    public record CartMedicineEventItem
    {
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
    }
}
