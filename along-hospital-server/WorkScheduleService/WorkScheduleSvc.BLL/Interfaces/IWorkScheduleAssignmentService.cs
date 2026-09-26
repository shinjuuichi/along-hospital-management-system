using SharedLibrary.Base.Services;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleAssignmentService
        : IBaseCrudService<CreateWorkScheduleAssignmentDTO, UpdateWorkScheduleAssignmentDTO, GetWorkScheduleAssignmentDTO>
    {
        Task<List<GetWorkScheduleAssignmentDTO>> BulkCreateAsync(CreateWorkScheduleAssignmentDTO dto);
    }
}
