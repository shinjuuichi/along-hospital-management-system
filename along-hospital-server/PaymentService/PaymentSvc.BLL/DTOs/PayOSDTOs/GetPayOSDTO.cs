using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PaymentSvc.BLL.DTOs.PayOSDTOs
{
    public class GetPayOSDTO : MapFrom<PayOSPayment>
    {
        public Guid TransactionId { get; set; }

        public string? PaymentUrl { get; set; }
    }
}