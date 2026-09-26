using SharedLibrary.Base.Services;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDTOs;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleTemplateService
        : IBaseCrudService<CreateWorkScheduleTemplateDTO, UpdateWorkScheduleTemplateDTO, GetWorkScheduleTemplateDTO>
    {
        Task DuplicateAsync(int id);
    }
}
