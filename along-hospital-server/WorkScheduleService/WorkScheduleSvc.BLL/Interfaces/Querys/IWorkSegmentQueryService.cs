using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;

namespace WorkScheduleSvc.BLL.Interfaces.Querys
{
    public interface IWorkSegmentQueryService
    {
        WorkSegmentPeriodDTO NormalizePeriod(DateTime periodStart, DateTime periodEnd);
        Task<List<GetWorkScheduleAssignmentForSegmentDTO>> GetAssignmentsForGenerationAsync(WorkSegmentPeriodDTO periodDTO);
    }
}
