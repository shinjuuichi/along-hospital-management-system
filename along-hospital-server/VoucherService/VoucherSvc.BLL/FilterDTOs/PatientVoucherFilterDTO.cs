using SharedLibrary.Commons.Filters;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.FilterDTOs
{
    public class PatientVoucherFilterDTO : MongoFilterDTO
    {
        [FilterField(FilterOperationEnum.Equal, nameof(PatientVoucher.PatientId))]
        public int? PatientId { get; set; }
    }
}
