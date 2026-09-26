using SharedLibrary.Commons.Filters;

namespace RecruitmentSvc.BLL.DTOs.FilterDTOs
{
    public class JobPostingFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? EmploymentType { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Role { get; set; }
    }
}