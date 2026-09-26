namespace PayrollSvc.BLL.DTOs.PayrollDTOs
{
    public class PaymentStatusChangedDTO
    {
        public Guid TransactionId { get; set; }

        public string? PaymentStatus { get; set; }
    }
}
