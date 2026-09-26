using AuthSvc.DAL.Enums;

namespace AuthSvc.BLL.Utils
{
    public static class VerificationDeliveryMethodResolver
    {
        public static VerificationDeliveryMethodEnum Resolve(
            VerificationDeliveryMethodEnum requestedMethod,
            string? email,
            string? phone)
        {
            if (requestedMethod == VerificationDeliveryMethodEnum.Email
                && string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(phone))
            {
                return VerificationDeliveryMethodEnum.Sms;
            }

            return requestedMethod;
        }
    }
}
