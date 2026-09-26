using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.VoucherContracts
{
    public record ApplyVoucherContract : BaseContract
    {
        public double OriginalTotalPrice { get; init; }

        public double FinalTotalPrice { get; init; }

        public List<MedicineDiscountDetailContract> MedicineDiscounts { get; init; } = [];

        public double PatientVoucherDiscountAmount { get; init; }

        public string? PatientVoucherCode { get; init; }

        public double TotalSaved => OriginalTotalPrice - FinalTotalPrice;
    }
}
