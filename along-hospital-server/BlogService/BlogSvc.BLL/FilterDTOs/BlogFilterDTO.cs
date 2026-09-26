using SharedLibrary.Commons.Filters;

namespace BlogSvc.BLL.FilterDTOs
{
    public class BlogFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Title { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? BlogCategoryId { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual)]
        public DateTime? CreationDate { get; set; }
    }
}