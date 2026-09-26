using AuthSvc.BLL.DTOs.Response;
using AuthSvc.DAL.Enums;
using AuthSvc.DAL.Models;
using SharedLibrary.Utils;

namespace AuthSvc.BLL.Utils
{
    public static class BuildVerificationMethodOptionsUtil
    {
        public static List<VerificationMethodOptionDTO> BuildVerificationMethodOptions(AuthAccount auth)
        {
            var methods = new List<VerificationMethodOptionDTO>();

            var email = StringUtil.NormalizeEmail(auth.Email ?? string.Empty);
            if (!string.IsNullOrWhiteSpace(email))
            {
                methods.Add(new VerificationMethodOptionDTO
                {
                    DeliveryMethod = VerificationDeliveryMethodEnum.Email.ToString(),
                    MaskedDestination = StringUtil.MaskEmail(email)
                });
            }

            var phone = auth.Phone?.Trim() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(phone))
            {
                methods.Add(new VerificationMethodOptionDTO
                {
                    DeliveryMethod = VerificationDeliveryMethodEnum.Sms.ToString(),
                    MaskedDestination = StringUtil.MaskPhone(phone)
                });
            }

            return methods;
        }
    }
}
