namespace BillingSvc.BLL.DTOs
{
    public class InvoiceAndChargeIdDTO(int invoiceId, int chargeId)
    {
        public int InvoiceId { get; set; } = invoiceId;

        public int ChargeId { get; set; } = chargeId;
    }
}
