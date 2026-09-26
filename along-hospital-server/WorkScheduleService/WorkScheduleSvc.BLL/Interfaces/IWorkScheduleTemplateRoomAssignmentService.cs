using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForStaffRoom;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleTemplateRoomAssignmentService
    {
        Task<List<GetWorkScheduleTemplateAssignmentForStaffRoomDTO>> GetRoomAssignmentsAsync(
            int templateId,
            int? shiftId,
            int? roomId,
            int? staffId);

        Task CreateRoomAssignmentsAsync(
            int templateId,
            CreateWorkScheduleTemplateAssignmentForStaffRoomDTO createDTO);

        Task DeleteRoomAssignmentAsync(int templateId, DeleteWorkScheduleTemplateAssignmentForStaffRoomDTO deleteDTO);
    }
}
