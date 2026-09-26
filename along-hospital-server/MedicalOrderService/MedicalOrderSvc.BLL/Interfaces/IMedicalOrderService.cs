using MedicalOrderSvc.BLL.DTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;

namespace MedicalOrderSvc.BLL.Interfaces
{
    public interface IMedicalOrderService
    {
        Task<GetMedicalOrderDTO> CreateAsync(CreateMedicalOrderWrapperDTO createWrapperDTO);
        Task<List<GetMedicalOrderDTO>> GetAllByMedicalHistoryIdAsync(int medicalHistoryId);
        Task CancelPendingOrDraftByMedicalHistoryIdAsync(int medicalHistoryId);
    }
}
