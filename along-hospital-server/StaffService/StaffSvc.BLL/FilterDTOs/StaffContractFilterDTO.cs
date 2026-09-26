using SharedLibrary.Commons.Filters;

namespace StaffSvc.BLL.FilterDTOs
{
    public class StaffContractFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? ContractCode { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? ContractType { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? StaffId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? RegionalWageId { get; set; }
    }
}
