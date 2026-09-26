using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs
{
    public class CreateWorkScheduleDTO : MapTo<WorkSchedule>
    {
        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public int? WorkScheduleTemplateId { get; set; }

        public int? ShiftId { get; set; }
    }
}