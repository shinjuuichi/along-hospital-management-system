using SharedLibrary.Commons.Filters;

namespace WorkScheduleSvc.BLL.FilterDTOs
{
    public class WorkScheduleFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public DateOnly? WorkDate { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? ShiftId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? WorkScheduleTemplateId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? WorkScheduleStatus { get; set; }
    }
}