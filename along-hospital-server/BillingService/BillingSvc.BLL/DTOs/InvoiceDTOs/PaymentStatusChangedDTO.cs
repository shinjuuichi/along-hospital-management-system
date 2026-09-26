namespace BillingSvc.BLL.DTOs.InvoiceDTOs
{
    public class PaymentStatusChangedDTO
    {
        public Guid TransactionId { get; set; }

        public string? PaymentStatus { get; set; }
    }
}
