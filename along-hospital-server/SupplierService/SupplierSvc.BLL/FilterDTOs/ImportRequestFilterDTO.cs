using SharedLibrary.Commons.Filters;
using SupplierSvc.DAL.Enums;

namespace SupplierSvc.BLL.FilterDTOs
{
    public class ImportRequestFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }
    }
}