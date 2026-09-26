using AuthSvc.DAL.Enums;
using AuthSvc.BLL.Models;

namespace AuthSvc.BLL.Interfaces
{
    public interface IVerificationCacheService
    {
        Task<bool> TryBeginCooldownAsync(
            VerificationPurposeEnum purpose,
            VerificationDeliveryMethodEnum deliveryMethod,
            string identifier,
            TimeSpan cooldown);

        Task<string> CreateEmailLinkTokenAsync(
            VerificationPurposeEnum purpose,
            string identifier,
            int authAccountId,
            TimeSpan expiration);

        Task<string> CreateSmsOtpAsync(
            VerificationPurposeEnum purpose,
            string identifier,
            int authAccountId,
            TimeSpan expiration);

        Task<VerificationCachePayload?> ConsumeEmailLinkTokenAsync(
            VerificationPurposeEnum purpose,
            string token);

        Task<VerificationCachePayload?> VerifySmsOtpAsync(
            VerificationPurposeEnum purpose,
            string identifier,
            string otp,
            int maxAttempts,
            bool consumeOnSuccess = true);

        Task<string> CreatePasswordResetSessionTokenAsync(
            int authAccountId,
            string identifier,
            TimeSpan expiration);

        Task<VerificationCachePayload?> ConsumePasswordResetSessionTokenAsync(string token);
    }
}
