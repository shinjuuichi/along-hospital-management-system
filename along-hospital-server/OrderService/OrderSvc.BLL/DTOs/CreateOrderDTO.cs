using OrderSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace OrderSvc.BLL.DTOs
{
    public class CreateOrderDTO : MapTo<Order>
    {
        public int PatientId { get; set; }
        public string? VoucherCode { get; set; }
        public bool IsPickupAtStore { get; set; }
        public string? PaymentType { get; set; }
        public string? Description { get; set; }
        public List<CreateOrderDetailDTO> Details { get; set; } = [];
    }
}
