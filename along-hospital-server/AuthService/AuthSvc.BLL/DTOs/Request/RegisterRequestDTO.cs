using AuthSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;
using System.ComponentModel.DataAnnotations;

namespace AuthSvc.BLL.DTOs.Request
{
    public class RegisterRequestDTO : MapTo<AuthAccount>
    {
        [MessageRequired]
        public string Phone { get; set; } = string.Empty;

        [MessageRequired, EmailValidator, MessageMaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [PasswordValidator]
        public string Password { get; set; } = string.Empty;

        [MessageRequired, PasswordValidator, MessageMaxLength(255)]
        [Compare("Password", ErrorMessage = "Confirm password does not match Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
