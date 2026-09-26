using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.InventoryContracts
{
    public record CreateInventoryContract : BaseContract
    {
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
        public int? MinQuantity { get; init; }
        public int? MaxQuantity { get; init; }
        public DateTime? LastImportDate { get; init; }
    }
}