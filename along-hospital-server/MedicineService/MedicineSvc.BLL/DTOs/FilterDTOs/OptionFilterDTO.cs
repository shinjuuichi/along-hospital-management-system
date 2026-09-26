using SharedLibrary.Commons.Filters;

namespace MedicineSvc.BLL.DTOs.FilterDTOs
{
    public class OptionFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? OptionName { get; set; }
    }
}
