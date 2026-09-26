using CartSvc.BLL.DTOs;

namespace CartSvc.BLL.Interfaces
{
    public interface ICartService
    {
        Task<GetCartDTO> GetCurrentUserCartAsync();
        Task CreateAsync(int patientId);
        Task AddToCartAsync(UpsertCartDetailDTO dto);
        Task UpdateDetailAsync(UpsertCartDetailDTO dto);
        Task DeleteDetailAsync(string skuCode);
        Task<GetPaymentUrlDTO> CheckoutAsync(CheckoutDTO checkoutDTO);
    }
}