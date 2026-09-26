using SharedLibrary.Commons.Filters;

namespace StaffSvc.BLL.FilterDTOs
{
    public class StaffCertificateTypeFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }
    }
}