namespace EmailSvc.DTOs
{
    public class SendInvoiceEmailDTO
    {
        public string? Email { get; set; }

        public string? PatientName { get; set; }

        public int InvoiceId { get; set; }

        public string? InvoiceNumber { get; set; }

        public int MedicalHistoryId { get; set; }

        public string? MedicalHistoryNumber { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double TotalInvoiceAmount { get; set; }

        public double TotalAmount { get; set; }

        public List<SendInvoiceEmailLineItemDTO> LineItems { get; set; } = [];
    }

    public class SendInvoiceEmailLineItemDTO
    {
        public string? ServiceName { get; set; }

        public string? ServiceDescription { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public double TotalAmount { get; set; }
    }
}