using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Commons.EntityAnnotations.RegExAttributes
{
    public class PhoneNumberValidatorAttribute : RegularExpressionAttribute
    {
        public PhoneNumberValidatorAttribute()
            : base(@"^0(2[0-9]{1,2}\d{7,8}|(3[2-9]|5[2-9]|7[0-9]|8[1-9]|9[0-9])\d{7})$")
        {
            ErrorMessage = "{0} is not valid. Must start with 0, and be a valid landline (02x) or mobile (03x/05x/07x/08x/09x).";
        }
    }
}
