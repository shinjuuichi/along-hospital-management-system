using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.ShiftDTOs
{
    public class UpdateShiftDTO : MapTo<Shift>
    {
        public string? Name { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool IsOvertime { get; set; }
    }
}