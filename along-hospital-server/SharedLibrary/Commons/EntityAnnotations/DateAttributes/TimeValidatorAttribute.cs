using SharedLibrary.Utils;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SharedLibrary.Commons.EntityAnnotations.DateAttributes
{
    /// <summary>
    /// Validates time values with flexible constraints for past/future times,
    /// time ranges, and cross-property comparisons. Supports TimeOnly, DateTime, and TimeSpan types.
    /// </summary>
    /// <example>
    /// <code>
    /// // Allow only future times (relative to current time of day)
    /// [TimeValidator(AllowPast = false)]
    /// public TimeOnly AppointmentTime { get; set; }
    ///
    /// // Allow only past times
    /// [TimeValidator(AllowFuture = false)]
    /// public TimeOnly CompletedTime { get; set; }
    ///
    /// // Ensure time is at least 30 minutes from now
    /// [TimeValidator(MinMinutesOffset = 30)]
    /// public TimeOnly ScheduledTime { get; set; }
    ///
    /// // Ensure time is within the next 120 minutes
    /// [TimeValidator(MaxMinutesOffset = 120)]
    /// public TimeOnly EventTime { get; set; }
    ///
    /// // Ensure time is between 30 minutes ago and 60 minutes from now
    /// [TimeValidator(MinMinutesOffset = -30, MaxMinutesOffset = 60)]
    /// public TimeOnly FlexibleTime { get; set; }
    ///
    /// // Ensure StartTime is not after EndTime
    /// [TimeValidator(NotAfter = nameof(EndTime))]
    /// public TimeOnly StartTime { get; set; }
    /// public TimeOnly EndTime { get; set; }
    ///
    /// // Ensure EndTime is not before StartTime
    /// [TimeValidator(NotBefore = nameof(StartTime))]
    /// public TimeOnly EndTime { get; set; }
    /// public TimeOnly StartTime { get; set; }
    ///
    /// // Restrict to a fixed time window (e.g., business hours 08:00–17:00)
    /// [TimeValidator(MinTime = "08:00", MaxTime = "17:00")]
    /// public TimeOnly OfficeTime { get; set; }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TimeValidatorAttribute : ValidationAttribute
    {
        public bool AllowPast { get; set; } = true;

        public bool AllowFuture { get; set; } = true;

        public int? MinMinutesOffset { get; set; } = null;

        public int? MaxMinutesOffset { get; set; } = null;

        /// <summary>
        /// Minimum allowed time as a string in "HH:mm" format.
        /// </summary>
        public string? MinTime { get; set; } = null;

        /// <summary>
        /// Maximum allowed time as a string in "HH:mm" format.
        /// </summary>
        public string? MaxTime { get; set; } = null;

        /// <summary>
        /// Name of the property that this time must not be after (i.e., this time &lt;= target time).
        /// </summary>
        public string? NotAfter { get; set; } = null;

        /// <summary>
        /// Name of the property that this time must not be before (i.e., this time &gt;= target time).
        /// </summary>
        public string? NotBefore { get; set; } = null;

        public TimeValidatorAttribute()
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            TimeOnly? converted = TimeUtil.TryConvertToTimeOnly(value);
            if (converted == null)
            {
                ErrorMessage = "{0} must be a valid time";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            TimeOnly time = converted.Value;
            var now = TimeOnly.FromDateTime(DateTime.Now);

            if (!AllowPast && time < now)
            {
                ErrorMessage = "{0} must not be in the past";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (!AllowFuture && time > now)
            {
                ErrorMessage = "{0} must not be in the future";
                return new ValidationResult(
                    FormatErrorMessage(validationContext.DisplayName),
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (MinMinutesOffset.HasValue)
            {
                var minTime = now.AddMinutes(MinMinutesOffset.Value);
                if (time < minTime)
                {
                    ErrorMessage = $"{{0}} must be on or after {minTime:HH:mm}";
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName),
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }
            }

            if (MaxMinutesOffset.HasValue)
            {
                var maxTime = now.AddMinutes(MaxMinutesOffset.Value);
                if (time > maxTime)
                {
                    ErrorMessage = $"{{0}} must be on or before {maxTime:HH:mm}";
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName),
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }
            }

            if (MinTime != null)
            {
                if (!TimeOnly.TryParse(MinTime, out var minTimeBound))
                {
                    return new ValidationResult(
                        $"Invalid MinTime format '{MinTime}'. Expected HH:mm.",
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }
                if (time < minTimeBound)
                {
                    ErrorMessage = $"{{0}} must not be before {minTimeBound:HH:mm}";
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName),
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }
            }

            if (MaxTime != null)
            {
                if (!TimeOnly.TryParse(MaxTime, out var maxTimeBound))
                {
                    return new ValidationResult(
                        $"Invalid MaxTime format '{MaxTime}'. Expected HH:mm.",
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }
                if (time > maxTimeBound)
                {
                    ErrorMessage = $"{{0}} must not be after {maxTimeBound:HH:mm}";
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName),
                        new[] { validationContext.MemberName ?? string.Empty }
                    );
                }
            }

            var notAfterResult = ValidateComparison(
                time, NotAfter, "must not be after", (current, other) => current > other, validationContext);
            if (notAfterResult != null) return notAfterResult;

            var notBeforeResult = ValidateComparison(
                time, NotBefore, "must not be before", (current, other) => current < other, validationContext);
            if (notBeforeResult != null) return notBeforeResult;

            return ValidationResult.Success;
        }

        private ValidationResult? ValidateComparison(
            TimeOnly currentTime,
            string? comparisonPropertyName,
            string errorVerb,
            Func<TimeOnly, TimeOnly, bool> failCondition,
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

            TimeOnly? comparisonTime = TimeUtil.TryConvertToTimeOnly(otherValue);
            if (comparisonTime == null)
            {
                return new ValidationResult(
                    $"Property '{comparisonPropertyName}' must be a valid time",
                    new[] { validationContext.MemberName ?? string.Empty }
                );
            }

            if (failCondition(currentTime, comparisonTime.Value))
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
