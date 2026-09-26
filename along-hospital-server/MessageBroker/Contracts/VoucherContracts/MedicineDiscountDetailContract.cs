using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.VoucherContracts
{
    public record MedicineDiscountDetailContract : BaseContract
    {
        public int MedicineId { get; init; }
        public string? SKUCode { get; init; }
        public double OriginalPrice { get; init; }
        public double MedicineDiscountAmount { get; init; }
        public double FinalPrice { get; init; }
    }
}
