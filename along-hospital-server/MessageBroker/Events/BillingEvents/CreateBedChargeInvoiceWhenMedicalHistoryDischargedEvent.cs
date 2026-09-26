using MessageBroker.Abstractions;

namespace MessageBroker.Events.BillingEvents
{
    public record CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }

        public List<CreateBedChargeInvoiceChargeItemEvent> Charges { get; init; } = [];

        public record CreateBedChargeInvoiceChargeItemEvent
        {
            public int MedicalServiceId { get; init; }

            public int Quantity { get; init; }
        }
    }

    public record CreateBedChargeInvoiceWhenMedicalHistoryDischargedContract : BaseContract;
}
