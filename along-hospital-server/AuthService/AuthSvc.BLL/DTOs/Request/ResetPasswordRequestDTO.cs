using System.ComponentModel.DataAnnotations;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace AuthSvc.BLL.DTOs.Request
{
    public class ResetPasswordRequestDTO
    {
        [MessageRequired, MessageMaxLength(255)]
        public string ResetToken { get; set; } = string.Empty;

        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        public string NewPassword { get; set; } = string.Empty;

        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match new password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
