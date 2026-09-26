using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendInvoiceEmailEvent : BaseEvent
    {
        public string? Email { get; init; }

        public string? PatientName { get; init; }

        public int InvoiceId { get; init; }

        public string? InvoiceNumber { get; init; }

        public int MedicalHistoryId { get; init; }

        public string? MedicalHistoryNumber { get; init; }

        public DateTime CreationDate { get; init; }

        public DateTime? PaymentDate { get; init; }

        public double TotalInvoiceAmount { get; init; }

        public double TotalAmount { get; init; }

        public List<SendInvoiceEmailLineItemEvent> LineItems { get; init; } = [];
    }

    public record SendInvoiceEmailLineItemEvent
    {
        public string? ServiceName { get; init; }

        public string? ServiceDescription { get; init; }

        public int Quantity { get; init; }

        public double UnitPrice { get; init; }

        public double TotalAmount { get; init; }
    }
}
