using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PaymentSvc.BLL.DTOs.SePayDTOs
{
    public class GetSePayDTO : MapFrom<SePayPayment>
    {
        public Guid TransactionId { get; set; }

        public string? PaymentUrl { get; set; }
    }
}