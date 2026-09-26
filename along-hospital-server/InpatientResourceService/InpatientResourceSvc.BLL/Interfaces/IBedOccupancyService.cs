using InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IBedOccupancyService
    {
        Task AssignBedAsync(AssignBedDTO assignBedDTO);
        Task TransferBedAsync(TransferBedDTO transferBedDTO);
        Task DischargeByMedicalHistoryIdAsync(int medicalHistoryId);
        Task<List<GetBedOccupancyDTO>> GetAllByMedicalHistoryIdAsync(int medicalHistoryId);
        Task<GetBedOccupancyDTO?> GetByMedicalHistoryIdAsync(int medicalHistoryId);
        Task<List<GetBedOccupancyBoardRoomDTO>> GetRoomBoardAsync();
    }
}
