using SharedLibrary.Commons.Filters;

namespace MedicalServiceSvc.BLL.DTOs
{
    public class MedicalServiceFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? SpecialtyId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? IsActive { get; set; }
    }
}
