namespace MessageBroker.Events.VoucherEvents
{
    public record MedicineDetailEventItem
    {
        public int MedicineId { get; init; }

        public double Price { get; init; }

        public int Quantity { get; init; }

        public string? SKUCode { get; init; }
    }
}
