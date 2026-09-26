using System.Security.Cryptography;
using System.Text;

namespace AuthSvc.BLL.Utils
{
    public static class OtpUtil
    {
        public static string GenerateOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var random = BitConverter.ToUInt32(bytes, 0);
            return (random % 900000 + 100000).ToString();
        }

        public static string GenerateOtpHash(string otp, string identifier, string secretKey)
        {
            var message = $"{otp}:{identifier.ToLowerInvariant()}";
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyOtp(string otp, string identifier, string storedHash, string secretKey)
        {
            var computedHash = GenerateOtpHash(otp, identifier, secretKey);
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedHash),
                Encoding.UTF8.GetBytes(storedHash));
        }
    }
}
