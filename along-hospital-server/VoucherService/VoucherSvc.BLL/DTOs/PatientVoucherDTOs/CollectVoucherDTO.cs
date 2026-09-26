using SharedLibrary.Commons.EntityAnnotations;

namespace VoucherSvc.BLL.DTOs.PatientVoucherDTOs
{
    public class CollectVoucherDTO
    {
        [MessageRequired]
        public string VoucherCode { get; set; } = string.Empty;
    }
}
