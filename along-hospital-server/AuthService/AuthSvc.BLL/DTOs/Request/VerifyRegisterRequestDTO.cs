using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace AuthSvc.BLL.DTOs.Request
{
    public class VerifyRegisterRequestDTO
    {
        [MessageRequired]
        public string DeliveryMethod { get; set; } = string.Empty;

        [MessageMaxLength(255)]
        public string? Identifier { get; set; }

        [OtpValidator]
        public string? Otp { get; set; }

        [MessageMaxLength(2048)]
        public string? Token { get; set; }
    }
}
