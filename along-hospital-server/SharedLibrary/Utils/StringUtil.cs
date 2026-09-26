using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SharedLibrary.Utils
{
    public static class StringUtil
    {
        private const int PASSWORD_LENGTH = 16;

        public static string GenerateRandomPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialCharacters = "@#$%^&";

            Random random = new();

            char[] passwordChars = new char[PASSWORD_LENGTH];
            passwordChars[0] = upperCase[random.Next(upperCase.Length)];
            passwordChars[1] = lowerCase[random.Next(lowerCase.Length)];
            passwordChars[2] = digits[random.Next(digits.Length)];
            passwordChars[3] = specialCharacters[random.Next(specialCharacters.Length)];

            string allCharacters = upperCase + lowerCase + digits + specialCharacters;
            for (int i = 4; i < PASSWORD_LENGTH; i++)
            {
                passwordChars[i] = allCharacters[random.Next(allCharacters.Length)];
            }

            return new string([.. passwordChars.OrderBy(c => random.Next())]);
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateSecureToken(int byteLength = 32)
        {
            var bytes = new byte[byteLength];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public static string? SplitWords(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return Regex.Replace(
                value,
                @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])",
                " "
            );
        }

        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (char.IsUpper(c))
                {
                    if (i > 0 && value[i - 1] != '-')
                        builder.Append('-');
                    builder.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public static string ToQueueName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            if (value.EndsWith("Event"))
                value = value[..^"Event".Length];
            else if (value.EndsWith("Command"))
                value = value[..^"Command".Length];
            else if (value.EndsWith("Contract"))
                value = value[..^"Contract".Length];

            return value.ToKebabCase();
        }

        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var lowerEmail = email.ToLowerInvariant();
            var parts = lowerEmail.Split('@');
            if (parts.Length != 2)
                return lowerEmail;

            var username = parts[0];
            var domain = parts[1];

            if (domain == "gmail.com" || domain == "googlemail.com")
            {
                var plusIndex = username.IndexOf('+');
                if (plusIndex != -1)
                {
                    username = username.Substring(0, plusIndex);
                }

                username = username.Replace(".", "");
            }

            return $"{username}@{domain}";
        }

        public static string NormalizePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            var trimmedPhone = phone.Trim();
            var digits = new string(trimmedPhone.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(digits))
                return string.Empty;

            if (trimmedPhone.StartsWith('+'))
                return $"+{digits}";

            if (digits.StartsWith("84"))
                return $"+{digits}";

            if (digits.StartsWith("0"))
                return $"+84{digits[1..]}";

            return digits;
        }

        public static string MaskEmail(string email)
        {
            var normalizedEmail = NormalizeEmail(email);
            if (string.IsNullOrWhiteSpace(normalizedEmail))
                return string.Empty;

            var parts = normalizedEmail.Split('@');
            if (parts.Length != 2)
                return "********";

            var local = parts[0];
            var domain = parts[1];
            var maskedLocal = local.Length switch
            {
                <= 0 => "********",
                1 => $"{local[0]}****",
                _ => $"{local[0]}{new string('*', Math.Min(2, local.Length - 1))}"
            };

            var domainParts = domain.Split('.');
            var primaryDomain = domainParts[0];
            var suffix = domainParts.Length > 1 ? $".{string.Join('.', domainParts.Skip(1))}" : string.Empty;
            var maskedDomain = string.IsNullOrWhiteSpace(primaryDomain)
                ? "********"
                : $"{primaryDomain[0]}{new string('*', Math.Max(3, primaryDomain.Length - 1))}";

            return $"{maskedLocal}@{maskedDomain}{suffix}";
        }

        public static string MaskPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            var digits = new string(phone.Where(char.IsDigit).ToArray());

            if (digits.Length == 0)
                return string.Empty;

            if (digits.Length <= 2)
                return digits;

            return $"{new string('*', Math.Max(3, digits.Length - 2))}{digits[^2..]}";
        }
    }
}
