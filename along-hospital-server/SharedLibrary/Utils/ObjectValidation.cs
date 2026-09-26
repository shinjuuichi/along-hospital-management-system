using SharedLibrary.Commons.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Utils
{
    public static class ObjectValidation
    {
        public static void TryValidate(this Object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (validationResults.Count != 0)
            {
                throw new ValidationFailureException(validationResults);
            }
        }
    }
}
