using SharedLibrary.Commons.Filters;

namespace StaffSvc.BLL.FilterDTOs
{
    public class SpecialtyFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }
    }
}