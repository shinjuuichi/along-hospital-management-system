using CartSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace CartSvc.BLL.DTOs
{
    public class UpsertCartDetailDTO : MapTo<CartDetail>
    {
        public string? SKUCode { get; set; }
        public int Quantity { get; set; }
    }
}
