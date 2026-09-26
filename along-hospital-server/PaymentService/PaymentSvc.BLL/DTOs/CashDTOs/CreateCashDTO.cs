using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PaymentSvc.BLL.DTOs.CashDTOs
{
    public class CreateCashDTO : MapTo<CashPayment>
    {
        public string? Description { get; set; }

        public List<CashItemDTO> CashItemDTOs { get; set; } = [];
    }

    public class CashItemDTO
    {
        public string? ServiceName { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }
    }
}