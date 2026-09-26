namespace AppointmentSvc.BLL.DTOs
{
    public class PaymentStatusChangedDTO
    {
        public Guid TransactionId { get; set; }

        public string? PaymentStatus { get; set; }
    }
}
