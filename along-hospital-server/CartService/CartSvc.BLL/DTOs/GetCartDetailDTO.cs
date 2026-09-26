using CartSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace CartSvc.BLL.DTOs
{
    public class GetCartDetailDTO : MapFrom<CartDetail>
    {
        public string? SKUCode { get; set; }
        public int Quantity { get; set; }
        public double? DiscountPrice { get; set; }
        public double OriginPrice { get; set; }
        public GetMedicineSKUDTO? MedicineSKU { get; set; }
    }
}
