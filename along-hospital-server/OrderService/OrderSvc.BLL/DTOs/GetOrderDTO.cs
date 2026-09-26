using OrderSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace OrderSvc.BLL.DTOs
{
    public class GetOrderDTO : MapFrom<Order>
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? VoucherCode { get; set; }
        public double OriginPrice { get; set; }
        public bool IsPickupAtStore { get; set; }
        public double? TotalDiscountAmount { get; set; }
        public double FinalPrice { get; set; }
        public string? PaymentUrl { get; set; }
        public List<GetOrderDetailDTO> OrderDetails { get; set; } = [];
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientPhone { get; set; }
        public string? PatientEmail { get; set; }
        public string? PatientImage { get; set; }
        public string? PatientAddress { get; set; }
    }
}
