using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.OrderContracts
{
    public record GetTopSellingMedicinesByDateRangeContract : BaseContract
    {
        public int TopN { get; init; }
        public List<GetTopSellingMedicineItemContract> Items { get; init; } = [];
    }

    public record GetTopSellingMedicineItemContract
    {
        public string SKUCode { get; init; } = string.Empty;
        public string MedicineName { get; init; } = string.Empty;
        public int QuantitySold { get; init; }
    }
}
