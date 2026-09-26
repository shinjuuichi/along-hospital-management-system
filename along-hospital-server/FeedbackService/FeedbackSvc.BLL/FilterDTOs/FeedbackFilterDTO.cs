using SharedLibrary.Commons.Filters;

namespace FeedbackSvc.BLL.FilterDTOs
{
    public class FeedbackFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? FeedbackReplyStatus { get; set; }
    }
}