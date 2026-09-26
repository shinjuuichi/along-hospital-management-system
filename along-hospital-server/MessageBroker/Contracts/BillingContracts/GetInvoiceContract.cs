using MessageBroker.Abstractions;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;

namespace MessageBroker.Contracts.BillingContracts
{
    public record GetInvoiceContract : BaseContract
    {
        public int Id { get; init; }

        public string? InvoiceNumber { get; init; }

        public string? InvoiceStatus { get; init; }

        public DateTime CreationDate { get; init; }

        public DateTime? PaymentDate { get; init; }

        public double TotalInvoiceAmount { get; init; }

        public double TotalRefundAmount { get; init; }

        public double TotalAmount { get; init; }

        public int MedicalHistoryId { get; init; }

        public string? ClinicalMedicalOrderId { get; init; }

        public List<GetChargeContract> Charges { get; init; } = [];

        public record GetChargeContract
        {
            public int Id { get; init; }

            public int Quantity { get; init; }

            public double UnitPrice { get; init; }

            public string? ChargeType { get; init; }

            public double TotalAmount { get; init; }

            public int InvoiceId { get; init; }

            public int MedicalServiceId { get; init; }

            public GetChargeSnapshotContract? ChargeSnapshot { get; init; }

            public GetRefundContract? Refund { get; init; }

            public record GetChargeSnapshotContract
            {
                public string? MedicalServiceName { get; init; }

                public string? MedicalServiceCode { get; init; }

                public string? MedicalServiceDescription { get; init; }
            }

            public record GetRefundContract
            {
                public int Id { get; init; }

                public string? Reason { get; init; }

                public string? RefundStatus { get; init; }

                public int? ApprovedBy { get; init; }

                public DateTime? ApprovalDate { get; init; }

                public int ChargeId { get; init; }

                public GetStaffDataByUserIdContract? Staff { get; init; }
            }
        }
    }

    public record GetListInvoiceDataContract : BaseContract
    {
        public List<GetInvoiceContract> Data { get; init; } = [];
    }
}