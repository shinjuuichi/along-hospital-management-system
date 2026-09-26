using SharedLibrary.Commons.Filters;

namespace StaffSvc.BLL.FilterDTOs
{
    public class StaffFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? SpecialtyId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? QualificationId { get; set; }

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }
    }
}