using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForTeleRoom;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleTemplateTeleRoomAssignmentService
    {
        Task<List<GetWorkScheduleTemplateAssignmentForStaffTeleRoomDTO>> GetTeleRoomAssignmentsAsync(
            int templateId,
            int? shiftId,
            int? teleRoomId,
            int? staffId);

        Task CreateTeleRoomAssignmentsAsync(
            int templateId,
            CreateWorkScheduleTemplateAssignmentForStaffTeleRoomDTO createDTO);

        Task DeleteTeleRoomAssignmentAsync(int templateId, DeleteWorkScheduleTemplateAssignmentForStaffTeleRoomDTO deleteDTO);
    }
}
