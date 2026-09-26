using SharedLibrary.Commons.Filters;

namespace PayrollSvc.BLL.FilterDTOs
{
    public class PayrollFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public int? Month { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? Year { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        public string? Name { get; set; }
    }
}
