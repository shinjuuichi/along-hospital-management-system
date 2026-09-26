using SharedLibrary.Utils;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharedLibrary.Commons.EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumberHigherThanOrEqualToAttribute : ValidationAttribute
    {
        private readonly double? _minValue;
        private readonly string? _comparisonProperty;

        public NumberHigherThanOrEqualToAttribute(double minValue)
        {
            _minValue = minValue;
            ErrorMessage = "{0} must be higher than or equal to {1}";
        }

        public NumberHigherThanOrEqualToAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
            ErrorMessage = "{0} must be higher than or equal to {1}";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            double? currentValue = TryConvertToDouble(value);
            if (currentValue == null)
                return new ValidationResult(
                    $"{validationContext.DisplayName} must be a numeric value",
                    new[] { validationContext.MemberName ?? string.Empty }
                );

            double comparisonValue;

            if (_comparisonProperty != null)
            {
                PropertyInfo? prop = validationContext.ObjectType.GetProperty(_comparisonProperty);
                if (prop == null)
                    return new ValidationResult(
                        $"Property '{_comparisonProperty}' not found",
                        new[] { validationContext.MemberName ?? string.Empty }
                    );

                object? otherValue = prop.GetValue(validationContext.ObjectInstance);
                double? converted = TryConvertToDouble(otherValue);
                if (converted == null)
                    return new ValidationResult(
                        $"Property '{_comparisonProperty}' must be numeric",
                        new[] { validationContext.MemberName ?? string.Empty }
                    );

                comparisonValue = converted.Value;
            }
            else if (_minValue.HasValue)
            {
                comparisonValue = _minValue.Value;
            }
            else
            {
                return new ValidationResult(
                    "Invalid comparison configuration",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (currentValue < comparisonValue)
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName, comparisonValue),
                    new[] { validationContext.MemberName ?? string.Empty }
                );

            return ValidationResult.Success;
        }

        private static double? TryConvertToDouble(object? value)
        {
            if (value == null) return null;
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return null;
            }
        }

        private string FormatErrorMessage(string name, double comparisonValue)
        {
            return string.Format(ErrorMessageString, name.SplitWords(), comparisonValue);
        }
    }
}
