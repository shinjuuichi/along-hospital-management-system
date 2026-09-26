using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.MedicineContracts
{
    public record GetMedicineSKUContract : BaseContract
    {
        public int Id { get; init; }

        public string? SKUCode { get; init; }

        public double Price { get; init; }

        public bool IsActive { get; init; }

        public int MedicineId { get; init; }

        public string? MedicineName { get; init; }

        public string? MedicineBrand { get; init; }

        public string? MedicineUnit { get; init; }

        public string[] MedicineImages { get; init; } = [];

        public int CategoryId { get; init; }

        public string? CategoryName { get; init; }

        public bool IsPublic { get; init; }

        public List<SKUValueContractItem> SkuValues { get; init; } = [];

        public int Quantity { get; init; }
    }

    public record SKUValueContractItem
    {
        public int OptionValueId { get; init; }

        public string? OptionName { get; init; }

        public string? ValueName { get; init; }

        public int UnitMultiplier { get; init; }
    }

    public record GetListMedicineSKUDataContract : BaseContract
    {
        public List<GetMedicineSKUContract> Data { get; init; } = [];
    }
}