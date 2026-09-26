using BillingSvc.BLL.DTOs;
using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.DAL.Enums;

namespace BillingSvc.BLL.Interfaces
{
    public interface IRefundService
    {
        Task<InvoiceAndChargeIdDTO> GetOrCreateInvoiceAndChargeIdByClinicalMedicalOrderDetailIdAsync(string clinicalMedicalOrderDetailId);
        Task<InvoiceAndChargeIdDTO> CreateByMedicalOrderDataAsync(string clinicalMedicalOrderId, CreateChargeDTO createChargeDTO);
        Task UpdateStatusByChargeIdAsync(int chargeId, RefundStatusEnum refundStatus);
        Task CancelExpiredRefundsAsync();
    }
}