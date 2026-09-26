using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.BillingContracts
{
    public record CreateRefundContract : BaseContract
    {
        public string? ClinicalMedicalOrderId { get; init; }

        public int MedicalServiceId { get; init; }

        public int Quantity { get; init; }

        public string? Reason { get; init; }

        public string? ClinicalMedicalOrderDetailId { get; init; }
    }
}
