using SharedLibrary.Commons.Filters;

namespace InpatientResourceSvc.BLL.FilterDTOs
{
    public class BuildingFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? Location { get; set; }
    }
}