using SharedLibrary.Commons.Filters;

namespace RecruitmentSvc.BLL.DTOs.FilterDTOs
{
    public class FilterJobApplicationDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public int? JobPostingId { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? Email { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? ApplicationStatus { get; set; }
    }
}
