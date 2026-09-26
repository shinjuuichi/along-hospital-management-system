namespace AuthSvc.BLL.Models
{
    public class VerificationCachePayload
    {
        public int AuthAccountId { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAtUtc { get; set; }
    }

    public class OtpVerificationCachePayload : VerificationCachePayload
    {
        public string OtpHash { get; init; } = string.Empty;
        public int AttemptCount { get; init; }
    }
}
