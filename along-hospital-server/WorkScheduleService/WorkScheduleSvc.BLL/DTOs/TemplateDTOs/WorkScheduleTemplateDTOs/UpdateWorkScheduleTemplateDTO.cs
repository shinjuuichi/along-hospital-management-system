using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDTOs
{
    public class UpdateWorkScheduleTemplateDTO : MapTo<WorkScheduleTemplate>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
