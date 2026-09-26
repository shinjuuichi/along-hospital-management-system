using PaymentSvc.BLL.DTOs.PayOSDTOs;
using PaymentSvc.DAL.Enums;

namespace PaymentSvc.BLL.Interfaces
{
    public interface IPayOSService
    {
        Task<GetPayOSDTO> CreatePayOSPaymentAsync(CreatePayOSDTO dto);
        Task UpdatePaymentStatus(long providerOrderCode, PaymentStatusEnum status);
        Task<string> GetPaymentUrlByTransactionIdAsync(Guid? transactionId);
    }
}