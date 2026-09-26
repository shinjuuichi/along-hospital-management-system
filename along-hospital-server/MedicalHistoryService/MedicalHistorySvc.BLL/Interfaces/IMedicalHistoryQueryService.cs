using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs;
using MedicalHistorySvc.BLL.FilterDTOs;
using SharedLibrary.Commons.Results;

namespace MedicalHistorySvc.BLL.Interfaces
{
    public interface IMedicalHistoryQueryService
    {
        Task<PaginationResult<GetMedicalHistoryDTO>> GetAllAsync(MedicalHistoryFilterDTO medicalHistoryFilterDTO);
        Task<PaginationResult<GetMedicalHistoryDTO>> GetAllByDoctorIdAsync(int doctorId, MedicalHistoryFilterDTO medicalHistoryFilterDTO);
        Task<PaginationResult<GetMedicalHistoryDTO>> GetAllByPatientIdAsync(int patientId, MedicalHistoryFilterDTO medicalHistoryFilterDTO);
        Task<List<GetMedicalHistoryDTO>> GetAllPendingAsync();
        Task<List<GetMedicalHistoryDTO>> GetAllByIdsAsync(List<int> ids);
        Task<List<GetMedicalHistoryDTO>> GetAllOccupancySummariesByIdsAsync(List<int> ids);

        Task<GetMedicalHistoryDTO> GetByIdAsync(int id, bool includeDataFromAnotherService = true);
        Task<bool> CheckExistByIdAsync(int id);
    }
}