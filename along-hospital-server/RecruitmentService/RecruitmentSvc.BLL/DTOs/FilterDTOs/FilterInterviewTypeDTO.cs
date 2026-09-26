using SharedLibrary.Commons.Filters;

namespace RecruitmentSvc.BLL.DTOs.FilterDTOs
{
    public class FilterInterviewTypeDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Name { get; set; }
    }
}
