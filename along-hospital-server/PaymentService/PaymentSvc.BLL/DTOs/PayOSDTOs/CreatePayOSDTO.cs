using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PaymentSvc.BLL.DTOs.PayOSDTOs
{
    public class CreatePayOSDTO : MapTo<PayOSPayment>
    {
        public string? Description { get; set; }

        public List<PayOsItemDTO> PayOsItemDTOs { get; set; } = [];
    }

    public class PayOsItemDTO
    {
        public string? ServiceName { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }
    }
}