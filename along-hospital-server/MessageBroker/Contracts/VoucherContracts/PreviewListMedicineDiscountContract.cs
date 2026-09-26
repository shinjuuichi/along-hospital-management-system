using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.VoucherContracts
{
    public record PreviewListMedicineDiscountContract : BaseContract
    {
        public List<MedicineDiscountDetailContract> MedicineDiscounts { get; init; } = [];
    }
}
