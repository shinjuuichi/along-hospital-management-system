using System.Text.RegularExpressions;

namespace PaymentSvc.BLL.Utils
{
    public static class GuidUtil
    {
        private static readonly Regex PrefixedGuid32Regex = new(@"\b[A-Za-z0-9_-]+(?<id>[0-9a-fA-F]{32})\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool TryGetTransactionIdFromOrderContent(string input, out Guid transactionId)
        {
            transactionId = Guid.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var match = PrefixedGuid32Regex.Match(input);
            if (!match.Success)
            {
                return false;
            }

            var idValue = match.Groups["id"].Value;
            return Guid.TryParseExact(idValue, "N", out transactionId);
        }
    }
}
