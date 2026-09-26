using SharedLibrary.Commons.Filters;

namespace InpatientResourceSvc.BLL.FilterDTOs
{
    public class RoomCategoryFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? Description { get; set; }
    }
}
