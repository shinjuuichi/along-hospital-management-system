namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs
{
    public class GetWorkScheduleRangeDTO
    {
        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }
    }
}
