using MedicalHistorySvc.BLL.Commons;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs;
using MedicalHistorySvc.DAL.Enums;

namespace MedicalHistorySvc.BLL.Interfaces
{
    public interface IMedicalHistoryCommandService
    {
        Task<GetMedicalHistoryDTO> CreateAsync(CreateMedicalHistoryDTO createMedicalHistoryDTO);

        Task AssignDoctorToMedicalHistoryAsync(int medicalHistoryId, int doctorId);
        Task DischargeInpatientBedAsync(int id);
        Task UpdateAsync(int id, UpdateMedicalHistoryDTO updateMedicalHistoryDTO);
        Task UpdateStatusAsync(int id, MedicalHistoryStatusEnum medicalHistoryStatus, string functionSource = FunctionSourceConstants.FROM_API);

        Task CancelPendingPaymentMedicalHistoriesAsync();
    }
}
