using System.Security.Cryptography;
using System.Text;

namespace AuthSvc.BLL.Utils
{
    public static class CryptoUtil
    {
        public static string EncryptPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string? password, string? passwordHashed)
        {
            return !string.IsNullOrEmpty(password)
                && !string.IsNullOrEmpty(passwordHashed)
                && BCrypt.Net.BCrypt.Verify(password, passwordHashed);
        }

        public static string GetSha256Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
