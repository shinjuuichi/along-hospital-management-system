using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations.RegExAttributes
{
    public class OtpValidatorAttribute : RegularExpressionAttribute
    {
        public OtpValidatorAttribute()
            : base(@"^\d{6}$")
        {
            ErrorMessage = "OTP must be exactly 6 digits.";
        }
    }
}