using SharedLibrary.Commons.Filters;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.FilterDTOs
{
    public class VoucherFilterDTO : MongoFilterDTO
    {
        [FilterField(FilterOperationEnum.Contains, nameof(Voucher.Name))]
        public string? Name { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(Voucher.VoucherStatus))]
        public string? VoucherStatus { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(Voucher.VoucherType))]
        public string? VoucherType { get; set; }
    }
}
