using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PaymentSvc.BLL.DTOs.SePayDTOs
{
    public class CreateSePayDTO : MapTo<SePayPayment>
    {
        public string? Description { get; set; }

        public string? BankCode { get; set; }

        public string? AccountNumber { get; set; }

        public List<SePayItemDTO> SePayItemDTOs { get; set; } = [];
    }

    public class SePayItemDTO
    {
        public string? ServiceName { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }
    }
}
