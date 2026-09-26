using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.HolidayDTOs
{
    public class UpsertHolidayDTO : MapTo<Holiday>
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int? Year { get; set; }
        public string? Name { get; set; }
    }
}