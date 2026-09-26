using SharedLibrary.Commons.Filters;

namespace ProductSvc.BLL.FilterDTOs
{
    public class ProductFilter : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string Name { get; set; } = string.Empty;

        [FilterField(FilterOperationEnum.Contains, TargetField: "Category.Name")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
