using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDayShiftDTOs;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleTemplateDayShiftService
    {
        Task<List<GetWorkScheduleTemplateDayShiftDTO>> GetDayShiftsAsync(int templateId);
        Task UpdateDayShiftsAsync(int templateId, List<UpdateWorkScheduleTemplateDayShiftDTO> dayShifts);
    }
}