using SharedLibrary.Commons.Filters;

namespace MedicineSvc.BLL.DTOs.FilterDTOs
{
    public class MedicineSKUFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public int? MedicineId { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? SKUCode { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? IsActive { get; set; }
    }
}