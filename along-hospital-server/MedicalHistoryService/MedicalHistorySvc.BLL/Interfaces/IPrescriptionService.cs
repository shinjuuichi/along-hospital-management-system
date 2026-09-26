using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.GetDTOs;
using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.UpsertDTOs;

namespace MedicalHistorySvc.BLL.Interfaces
{
    public interface IPrescriptionService
    {
        Task<GetPrescriptionDTO> CreateAsync(int medicalHistoryId, UpsertPrescriptionDTO upsertPrescriptionDTO);
        Task<GetPrescriptionDTO> UpdateByMedicalHistoryIdAsync(int medicalHistoryId, UpsertPrescriptionDTO upsertPrescriptionDTO);
    }
}
