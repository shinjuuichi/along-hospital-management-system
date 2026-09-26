using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkSegmentService
    {
        Task<List<GetWorkSegmentDTO>> GetByAssignmentIdsAsync(List<int> assignmentIds);
        Task GenerateAsync(DateTime periodStart, DateTime periodEnd);
        Task<GetWorkSegmentDTO> UpdateAsync(int id, UpdateWorkSegmentDTO dto);
    }
}
