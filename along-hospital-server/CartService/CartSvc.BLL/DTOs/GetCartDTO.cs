using CartSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace CartSvc.BLL.DTOs
{
    public class GetCartDTO : MapFrom<Cart>
    {
        public int Id { get; set; }
        public List<GetCartDetailDTO> CartDetails { get; set; } = [];
    }
}
