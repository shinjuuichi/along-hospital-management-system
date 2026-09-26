using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations.RegExAttributes
{
    public class NotContainsSpecialCharacterValidatorAttribute : RegularExpressionAttribute
    {
        public NotContainsSpecialCharacterValidatorAttribute()
            : base(@"^[^`~!@#$%^&*()_|+\-=?;:'"",.<>{}\[\]\\\/]+$")
        {
            ErrorMessage = "{0} must not contain special characters.";
        }
    }
}
