namespace WorkScheduleSvc.BLL.DTOs.HolidayDTOs
{
    public class CreateHolidayRangeDTO
    {
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string? Name { get; set; }
    }
}