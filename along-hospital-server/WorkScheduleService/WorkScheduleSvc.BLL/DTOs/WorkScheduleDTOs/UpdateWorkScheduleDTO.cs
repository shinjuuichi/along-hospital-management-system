using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs
{
    public class UpdateWorkScheduleDTO : MapTo<WorkSchedule>
    {
        public DateOnly WorkDate { get; set; }

        public int ShiftId { get; set; }
    }
}