using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SharedLibrary.Commons.EntityAnnotations.RegExAttributes
{
    public class EmailOrPhoneValidatorAttribute : ValidationAttribute
    {
        private const string EmailPattern = @"^(?=.{1,320}$)(?!.*\.\.)[a-zA-Z0-9](?:[a-zA-Z0-9._%+\-]{0,62}[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9\-]{0,253}[a-zA-Z0-9])?(?:\.[a-zA-Z]{2,})+$";
        private const string PhonePattern = @"^0(2[0-9]{1,2}\d{7,8}|(3[2-9]|5[2-9]|7[0-9]|8[1-9]|9[0-9])\d{7})$";

        public EmailOrPhoneValidatorAttribute()
        {
            ErrorMessage = "Identifier must be a valid email or phone number";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult(ErrorMessage);
            }

            var str = value.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(str))
            {
                return new ValidationResult(ErrorMessage);
            }

            var isEmail = Regex.IsMatch(str, EmailPattern, RegexOptions.IgnoreCase);
            var isPhone = Regex.IsMatch(str, PhonePattern);

            if (!isEmail && !isPhone)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
