using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace AuthSvc.BLL.DTOs.Request
{
    public class ForgotPasswordRequestDTO
    {
        [MessageRequired, EmailOrPhoneValidator]
        public string Identifier { get; set; } = string.Empty;

        [MessageRequired]
        public string DeliveryMethod { get; set; } = string.Empty;
    }
}
