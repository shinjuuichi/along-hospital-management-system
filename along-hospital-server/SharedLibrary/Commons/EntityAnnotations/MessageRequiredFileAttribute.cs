using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageRequiredFileAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return new ValidationResult(
                    "File is required.",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (file.Length == 0)
            {
                return new ValidationResult(
                    "Uploaded file cannot be empty.",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            return ValidationResult.Success;
        }
    }
}
