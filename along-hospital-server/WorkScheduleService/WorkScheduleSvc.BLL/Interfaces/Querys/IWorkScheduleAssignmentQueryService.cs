using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Interfaces.Querys
{
    public interface IWorkScheduleAssignmentQueryService
    {
        Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsAsync(int workScheduleId);
        Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsByWorkScheduleIdsAsync(List<int> workScheduleIds);
        Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsByStaffIdAsync(int staffId);
        Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsByCurrentUserAsync();
        Task<PaginationResult<GetWorkScheduleAssignmentDTO>> GetAllPaginatedAsync(FilterDTO filterDTO);
        Task<List<GetWorkScheduleAssignmentDTO>> BuildAssignmentDTOsAsync(List<WorkScheduleAssignment> assignments);
    }
}
