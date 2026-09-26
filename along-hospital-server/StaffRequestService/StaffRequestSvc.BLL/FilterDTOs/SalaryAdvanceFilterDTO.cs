using SharedLibrary.Commons.Filters;

namespace StaffRequestSvc.BLL.FilterDTOs
{
    public class SalaryAdvanceFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public int? CreatedBy { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }
    }
}