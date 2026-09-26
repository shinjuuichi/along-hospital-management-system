using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.HolidayDTOs
{
    public class GetHolidayDTO : MapFrom<Holiday>
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int? Year { get; set; }
        public string? Name { get; set; }
    }
}