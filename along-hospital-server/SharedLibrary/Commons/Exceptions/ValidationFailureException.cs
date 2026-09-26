using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.Exceptions
{
    public class ValidationFailureException : Exception
    {
        public IDictionary<string, string> FieldErrors { get; } = new Dictionary<string, string>();
        public IList<string> GlobalErrors { get; } = [];

        public ValidationFailureException() : base("Validation failed") { }

        public ValidationFailureException(string field, string errorMessage) : this()
        {
            if (!string.IsNullOrWhiteSpace(field))
            {
                FieldErrors[field] = string.IsNullOrWhiteSpace(errorMessage) ? "Invalid" : errorMessage;
            }
            else if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                GlobalErrors.Add(errorMessage);
            }
        }

        public ValidationFailureException(string? errorMessage) : this()
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                GlobalErrors.Add(errorMessage);
            }
        }

        public ValidationFailureException(IEnumerable<ValidationResult> results) : this()
        {
            foreach (var r in results)
            {
                if (r.MemberNames != null && r.MemberNames.Any())
                {
                    foreach (var name in r.MemberNames)
                    {
                        if (!FieldErrors.ContainsKey(name))
                        {
                            FieldErrors[name] = r.ErrorMessage ?? "Invalid";
                        }
                    }
                }
                else
                {
                    GlobalErrors.Add(r.ErrorMessage ?? "Invalid");
                }
            }
        }

        public ValidationFailureException(IDictionary<string, string> fieldErrors) : this()
        {
            foreach (var kv in fieldErrors)
            {
                if (!FieldErrors.ContainsKey(kv.Key))
                {
                    FieldErrors[kv.Key] = kv.Value;
                }
            }
        }

        public ValidationFailureException(IList<string> globalErrors) : this()
        {
            foreach (var e in globalErrors)
            {
                GlobalErrors.Add(e);
            }
        }
    }
}