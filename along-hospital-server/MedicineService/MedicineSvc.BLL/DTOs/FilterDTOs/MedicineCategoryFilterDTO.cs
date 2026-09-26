using SharedLibrary.Commons.Filters;

namespace MedicineSvc.BLL.DTOs.FilterDTOs
{
    public class MedicineCategoryFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }
    }
}
