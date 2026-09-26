using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.InventoryContracts
{
    public record GetInventoryBySKUCodeContract : BaseContract
    {
        public int Id { get; init; }
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
        public int? MinQuantity { get; init; }
        public int? MaxQuantity { get; init; }
        public DateTime? LastImportDate { get; init; }
    }

    public record GetListInventoryDataBySKUCodesContract : BaseContract
    {
        public List<GetInventoryBySKUCodeContract> Data { get; init; } = [];
    }
}
