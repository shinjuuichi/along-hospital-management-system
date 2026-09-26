using SharedLibrary.Commons.Filters;

namespace MedicineSvc.BLL.DTOs.FilterDTOs
{
    public class MedicineFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public int? MedicineCategoryId { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? MedicineUnitId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? IsPublic { get; set; }
    }
}