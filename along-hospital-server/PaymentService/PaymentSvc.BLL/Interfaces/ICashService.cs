using PaymentSvc.BLL.DTOs.CashDTOs;

namespace PaymentSvc.BLL.Interfaces
{
    public interface ICashService
    {
        Task<GetCashDTO> CreateCashPaymentAsync(CreateCashDTO createCashDTO);
        Task UpdatePaymentStatus(Guid transactionId);
    }
}