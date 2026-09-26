using System.ComponentModel.DataAnnotations;
using SharedLibrary.Utils;

namespace SharedLibrary.Commons.EntityAnnotations.DateAttributes
{
    /// <summary>
    /// Validates that a date value represents an age within a specified range.
    /// Supports both DateTime and DateOnly types.
    /// Default range: 0 to 122 years old.
    /// </summary>
    /// <example>
    /// <code>
    /// // Accept ages from 0 to 122 (default)
    /// [AgeRange]
    /// public DateTime DateOfBirth { get; set; }
    /// 
    /// // Accept ages from 18 and above (up to 122)
    /// [AgeRange(Min = 18)]
    /// public DateOnly DateOfBirth { get; set; }
    /// 
    /// // Accept ages from 18 to 65 only
    /// [AgeRange(Min = 18, Max = 65)]
    /// public DateOnly DateOfBirth { get; set; }
    /// 
    /// // Accept ages from 18 to 65 only (shorthand)
    /// [AgeRange(18, 65)]
    /// public DateOnly DateOfBirth { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AgeRangeAttribute : ValidationAttribute
    {
        public const int DefaultMinAge = 0;
        public const int DefaultMaxAge = 122;

        public int Min { get; set; } = DefaultMinAge;
        public int Max { get; set; } = DefaultMaxAge;

        public AgeRangeAttribute()
        {
        }

        public AgeRangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (Min < 0)
            {
                ErrorMessage = $"{{0}} minimum age cannot be negative";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (Max < 0)
            {
                ErrorMessage = $"{{0}} maximum age cannot be negative";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (Min > DefaultMaxAge)
            {
                ErrorMessage = $"{{0}} minimum age cannot exceed {DefaultMaxAge} years old";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (Max > DefaultMaxAge)
            {
                ErrorMessage = $"{{0}} maximum age cannot exceed {DefaultMaxAge} years old";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (Min > Max)
            {
                ErrorMessage = $"{{0}} minimum age cannot be greater than maximum age";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (value == null)
            {
                return ValidationResult.Success;
            }

            DateTime? converted = TimeUtil.TryConvertToDateTime(value);
            if (converted == null)
            {
                ErrorMessage = "{0} must be a valid date";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            DateTime dateOfBirth = converted.Value;

            var age = CalculateAge(dateOfBirth);

            if (age < Min)
            {
                ErrorMessage = $"{{0}} must represent an age of at least {Min} years old";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]
                );
            }

            if (age > Max)
            {
                ErrorMessage = $"{{0}} must represent an age of at most {Max} years old";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]
                );
            }

            return ValidationResult.Success;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Now.Date;

            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name.SplitWords());
        }
    }
}
