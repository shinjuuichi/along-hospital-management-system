using SharedLibrary.Commons.Filters;

namespace WorkScheduleSvc.BLL.FilterDTOs;

public class ShiftFilterDTO : FilterDTO
{
    [FilterField(FilterOperationEnum.Contains)]
    public string? Name { get; set; }

    [FilterField(FilterOperationEnum.GreaterThanOrEqual)]
    public TimeOnly? StartTime { get; set; }

    [FilterField(FilterOperationEnum.LessThanOrEqual)]
    public TimeOnly? EndTime { get; set; }
}