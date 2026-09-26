using SharedLibrary.Utils;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MessageRangeAttribute : RangeAttribute
    {
        private readonly double _minimum;
        private readonly double _maximum;

        public MessageRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
            ErrorMessage = "{0} must be between {1} and {2}";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var number = TryConvertToDouble(value);
            if (number == null)
            {
                return ValidationResult.Success;
            }

            if (number < _minimum || number > _maximum)
            {
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    [validationContext.MemberName ?? string.Empty]
                );
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name.SplitWords(), _minimum, _maximum);
        }

        private static double? TryConvertToDouble(object? value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return null;
            }
        }
    }
}
