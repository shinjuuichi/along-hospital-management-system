using SharedLibrary.Commons.Filters;

namespace MedicineSvc.BLL.DTOs.FilterDTOs
{
    public class OptionValueFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? IsActive { get; set; }
    }
}
