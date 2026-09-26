using SharedLibrary.Commons.Filters;

namespace ProductSvc.BLL.FilterDTOs
{
    public class CategoryFilter : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string Name { get; set; } = string.Empty;
    }
}
