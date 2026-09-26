using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PaymentSvc.BLL.DTOs.CashDTOs
{
    public class GetCashDTO : MapFrom<CashPayment>
    {
        public Guid TransactionId { get; set; }
    }
}