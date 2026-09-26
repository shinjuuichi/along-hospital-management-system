using SharedLibrary.Commons.Filters;

namespace StaffSvc.BLL.FilterDTOs
{
    public class StaffGroupFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }
    }
}