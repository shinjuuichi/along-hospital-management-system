using SharedLibrary.Commons.Filters;

namespace StaffRequestSvc.BLL.FilterDTOs
{
    public class LeaveRequestFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public int? CreatedBy { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? LeaveType { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual)]
        public DateOnly? FromDate { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual)]
        public DateOnly? ToDate { get; set; }
    }
}