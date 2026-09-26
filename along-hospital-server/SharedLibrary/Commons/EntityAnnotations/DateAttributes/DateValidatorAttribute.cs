using SharedLibrary.Utils;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharedLibrary.Commons.EntityAnnotations.DateAttributes
{
    /// <summary>
    /// Validates date values with flexible constraints for past/future dates, date ranges,
    /// and cross-property comparisons. Supports both DateTime and DateOnly types.
    /// </summary>
    /// <example>
    /// <code>
    /// // Allow only future dates
    /// [DateValidator(AllowPast = false)]
    /// public DateTime AppointmentDate { get; set; }
    /// 
    /// // Allow only past dates
    /// [DateValidator(AllowFuture = false)]
    /// public DateTime CompletedDate { get; set; }
    ///
    /// // Allow dates within 7-30 days from now
    /// [DateValidator(MinDaysOffset = 7, MaxDaysOffset = 30)]
    /// public DateOnly ScheduledDate { get; set; }
    ///
    /// // Allow dates starting from tomorrow (at least 1 day in future)
    /// [DateValidator(MinDaysOffset = 1)]
    /// public DateTime EventDate { get; set; }
    ///
    /// // Allow dates within next 90 days only
    /// [DateValidator(MaxDaysOffset = 90)]
    /// public DateOnly ExpiryDate { get; set; }
    ///
    /// // Allow dates from 3 days ago to 7 days in future
    /// [DateValidator(MinDaysOffset = -3, MaxDaysOffset = 7)]
    /// public DateTime FlexibleDate { get; set; }
    ///
    /// // Ensure FromDate is not after ToDate
    /// [DateValidator(NotAfter = nameof(ToDate))]
    /// public DateTime FromDate { get; set; }
    /// public DateTime ToDate { get; set; }
    ///
    /// // Ensure EndDate is not before StartDate
    /// [DateValidator(NotBefore = nameof(StartDate))]
    /// public DateTime EndDate { get; set; }
    /// public DateTime StartDate { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateValidatorAttribute : ValidationAttribute
    {
        public bool AllowPast { get; set; } = true;

        public bool AllowFuture { get; set; } = true;

        public int? MinDaysOffset { get; set; } = null;

        public int? MaxDaysOffset { get; set; } = null;

        /// <summary>
        /// Name of the property that this date must not be after (i.e., this date &lt;= target date).
        /// </summary>
        public string? NotAfter { get; set; } = null;

        /// <summary>
        /// Name of the property that this date must not be before (i.e., this date &gt;= target date).
        /// </summary>
        public string? NotBefore { get; set; } = null;

        public DateValidatorAttribute()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
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

            DateTime date = converted.Value;

            var today = DateTime.Now.Date;

            if (!AllowPast && date < today)
            {
                ErrorMessage = "{0} must not be in the past";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (!AllowFuture && date > today)
            {
                ErrorMessage = "{0} must not be in the future";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (MinDaysOffset.HasValue && date < today.AddDays(MinDaysOffset.Value))
            {
                var minDate = today.AddDays(MinDaysOffset.Value);
                ErrorMessage = $"{{0}} must be on or after {minDate:yyyy-MM-dd}";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (MaxDaysOffset.HasValue && date > today.AddDays(MaxDaysOffset.Value))
            {
                var maxDate = today.AddDays(MaxDaysOffset.Value);
                ErrorMessage = $"{{0}} must be on or before {maxDate:yyyy-MM-dd}";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            var notAfterResult = this.ValidateComparison(
                date, NotAfter, "must not be after", (current, other) => current > other, validationContext);
            if (notAfterResult != null) return notAfterResult;

            var notBeforeResult = this.ValidateComparison(
                date, NotBefore, "must not be before", (current, other) => current < other, validationContext);
            if (notBeforeResult != null) return notBeforeResult;

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateComparison(
            DateTime currentDate,
            string? comparisonPropertyName,
            string errorVerb,
            Func<DateTime, DateTime, bool> failCondition,
            ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(comparisonPropertyName)) return null;

            PropertyInfo? prop = validationContext.ObjectType.GetProperty(comparisonPropertyName);
            if (prop == null)
            {
                return new ValidationResult(
                    $"Property '{comparisonPropertyName}' not found",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            object? otherValue = prop.GetValue(validationContext.ObjectInstance);
            if (otherValue == null) return null;

            DateTime? comparisonDate = TimeUtil.TryConvertToDateTime(otherValue);
            if (comparisonDate == null)
            {
                return new ValidationResult(
                    $"Property '{comparisonPropertyName}' must be a valid date",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (failCondition(currentDate, comparisonDate.Value))
            {
                ErrorMessage = $"{{0}} {errorVerb} {{1}}";
                return new ValidationResult(
                    string.Format(ErrorMessageString, validationContext.DisplayName.SplitWords(), comparisonPropertyName.SplitWords()),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name.SplitWords());
        }
    }
}