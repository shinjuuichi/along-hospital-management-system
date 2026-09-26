using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.DAL.Enums;
using MessageBroker.Contracts.MedicalOrderContracts;
using SharedLibrary.Base.Services;

namespace BillingSvc.BLL.Interfaces
{
    public interface IInvoiceService : IBaseCrudService<CreateInvoiceDTO, object, GetInvoiceDTO>
    {
        Task<List<GetInvoiceDTO>> GetAllByMedicalHistoryIdAsync(int medicalHistoryId);

        Task<List<GetInvoiceDTO>> GetOrCreateAllPendingInvoicesByClinicalMedicalOrderIdsAsync(Dictionary<string, GetClinicalMedicalOrderContract> clinicalMedicalOrderDict);

        Task<string> GetPaymentUrlByInvoiceIdAsync(int invoiceId);

        Task CreateGeneralInvoiceAsync(CreateGeneralInvoiceDTO createGeneralInvoiceDTO);

        Task UpdateStatusAsync(int invoiceId, InvoiceStatusEnum invoiceStatus);

        Task CancelAllPendingByMedicalHistoryIdAsync(int medicalHistoryId);

        Task CancelPendingByClinicalMedicalOrderIdAsync(string clinicalMedicalOrderId);

        Task CancelPendingByClinicalMedicalOrderIdsAsync(List<string> clinicalMedicalOrderIds);

        Task HandlePaymentStatusChangedAsync(PaymentStatusChangedDTO paymentStatusChangedDTO);
    }
}