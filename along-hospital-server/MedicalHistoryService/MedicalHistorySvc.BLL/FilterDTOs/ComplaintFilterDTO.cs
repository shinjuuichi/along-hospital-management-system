using SharedLibrary.Commons.Filters;

namespace MedicalHistorySvc.BLL.FilterDTOs
{
    public class ComplaintFilterDTO : FilterDTO
    {
        [FilterField]
        public string? ComplaintTopic { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? Content { get; set; }

        [FilterField]
        public string? ComplaintType { get; set; }

        [FilterField]
        public string? ComplaintResolveStatus { get; set; }
    }
}
