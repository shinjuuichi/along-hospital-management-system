using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations.RegExAttributes
{
    public class MedicalNumberValidatorAttribute : RegularExpressionAttribute
    {
        public MedicalNumberValidatorAttribute() : base(@"^[A-Z0-9\-]+$")
        {
            ErrorMessage = "Medical number must contain only uppercase letters, digits, or '-' characters.";
        }
    }
}
