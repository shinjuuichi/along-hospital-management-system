using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;
using System.ComponentModel.DataAnnotations;

namespace AuthSvc.BLL.DTOs.Request
{
    public class ChangePasswordRequestDTO
    {
        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        public string CurrentPassword { get; set; } = string.Empty;

        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        public string NewPassword { get; set; } = string.Empty;

        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password does not match new password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
