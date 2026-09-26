using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace AuthSvc.BLL.DTOs.Request
{
    public class LoginRequestDTO
    {
        [MessageRequired, MessageMaxLength(255)]
        [EmailOrPhoneValidator]
        public string Identifier { get; set; } = string.Empty;

        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        public string Password { get; set; } = string.Empty;
    }
}
