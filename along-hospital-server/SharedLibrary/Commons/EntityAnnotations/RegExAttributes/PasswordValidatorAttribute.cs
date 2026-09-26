using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations.RegExAttributes
{
    public class PasswordValidatorAttribute : RegularExpressionAttribute
    {
        public PasswordValidatorAttribute()
            : base(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
        {
            ErrorMessage = "Password must be at least 8 characters, include upper/lowercase, number and special character.";
        }
    }
}