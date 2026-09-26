using System.ComponentModel.DataAnnotations;
using SharedLibrary.Utils;

namespace SharedLibrary.Commons.EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageRequiredAttribute : RequiredAttribute
    {
        public MessageRequiredAttribute()
        {
            ErrorMessage = "{0} in {1} is required";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName, validationContext.ObjectType.Name),
                    [validationContext.MemberName ?? string.Empty]
                );
            }

            return ValidationResult.Success;
        }

        public string FormatErrorMessage(string fieldName, string className)
        {
            return string.Format(ErrorMessageString, fieldName.SplitWords(), className.SplitWords());
        }
    }
}
