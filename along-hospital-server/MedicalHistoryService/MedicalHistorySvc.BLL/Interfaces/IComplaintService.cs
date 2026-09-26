using MedicalHistorySvc.BLL.DTOs.ComplaintDTOs;
using MedicalHistorySvc.BLL.FilterDTOs;
using MedicalHistorySvc.DAL.Enums;
using SharedLibrary.Commons.Results;

namespace MedicalHistorySvc.BLL.Interfaces
{
    public interface IComplaintService
    {
        // Complaint Methods
        Task<PaginationResult<GetComplaintDTO>> GetAllAsync(ComplaintFilterDTO complaintFilterDTO);
        Task<GetComplaintDTO> CreateAsync(int medicalHistoryId, CreateComplaintDTO createComplaintDTO);
        Task UpdateStatusAsync(int complaintId, ComplaintResolveStatusEnum complaintResolveStatus, string? response = null);
        Task ClassifyAsync(int complaintId, ComplaintTypeEnum complaintType);

        // Complaint Summary Methods
        Task CreateComplaintSummaryForLastWeekAsync();
        Task<string> GetComplaintSummaryByWeekAsync(string yearWeekString);

        // AI-Related Methods
        Task GetTypePredictionAsync(int complaintId);
        Task RetrainTypePredictionModelAsync();
    }
}