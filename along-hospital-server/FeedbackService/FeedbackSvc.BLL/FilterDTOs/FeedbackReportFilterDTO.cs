using SharedLibrary.Commons.Filters;

namespace FeedbackSvc.BLL.FilterDTOs
{
    public class FeedbackReportFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }
    }
}