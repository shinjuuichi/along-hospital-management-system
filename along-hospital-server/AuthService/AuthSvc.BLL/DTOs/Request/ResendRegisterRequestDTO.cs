using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace AuthSvc.BLL.DTOs.Request
{
    public class ResendRegisterRequestDTO
    {
        [MessageRequired, MessageMaxLength(255)]
        [EmailOrPhoneValidator]
        public string Identifier { get; set; } = string.Empty;

        [MessageRequired]
        public string DeliveryMethod { get; set; } = string.Empty;
    }
}
