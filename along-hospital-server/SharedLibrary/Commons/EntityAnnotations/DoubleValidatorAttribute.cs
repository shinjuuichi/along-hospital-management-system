using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharedLibrary.Commons.EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DoubleValidatorAttribute : ValidationAttribute
    {
        /// <summary>
        /// This value must not be greater than the target property.
        /// </summary>
        public string? NotGreaterThan { get; set; }

        /// <summary>
        /// This value must not be less than the target property.
        /// </summary>
        public string? NotLessThan { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!double.TryParse(value.ToString(), out double current))
            {
                ErrorMessage = "{0} must be a valid number";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty });
            }

            var notGreaterResult = ValidateComparison(
                current,
                NotGreaterThan,
                "must not be greater than",
                (a, b) => a > b,
                validationContext);

            if (notGreaterResult != null)
            {
                return notGreaterResult;
            }

            var notLessResult = ValidateComparison(
                current,
                NotLessThan,
                "must not be less than",
                (a, b) => a < b,
                validationContext);

            if (notLessResult != null)
            {
                return notLessResult;
            }

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateComparison(
            double current,
            string? propertyName,
            string errorVerb,
            Func<double, double, bool> failCondition,
            ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            PropertyInfo? prop = validationContext.ObjectType.GetProperty(propertyName);
            if (prop == null)
            {
                return new ValidationResult(
                    $"Property '{propertyName}' not found",
                    new[] { validationContext.MemberName ?? string.Empty });
            }

            object? otherValue = prop.GetValue(validationContext.ObjectInstance);
            if (otherValue == null)
            {
                return null;
            }

            if (!double.TryParse(otherValue.ToString(), out double other))
            {
                return new ValidationResult(
                    $"Property '{propertyName}' must be a valid number",
                    new[] { validationContext.MemberName ?? string.Empty });
            }

            if (failCondition(current, other))
            {
                ErrorMessage = $"{{0}} {errorVerb} {{1}}";
                return new ValidationResult(
                    string.Format(ErrorMessageString, validationContext.DisplayName, propertyName),
                    new[] { validationContext.MemberName ?? string.Empty });
            }

            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}