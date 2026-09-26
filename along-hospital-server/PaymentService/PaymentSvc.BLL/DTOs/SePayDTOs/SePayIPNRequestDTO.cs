using System.Text.Json.Serialization;
using PaymentSvc.BLL.Utils;

namespace PaymentSvc.BLL.DTOs.SePayDTOs
{
    public sealed class SePayIPNRequestDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }

        [JsonPropertyName("gateway")]
        public string? Gateway { get; init; }

        [JsonPropertyName("transactionDate")]
        [JsonConverter(typeof(SePayDateTimeConverter))]
        public DateTime TransactionDate { get; init; }

        [JsonPropertyName("accountNumber")]
        public string? AccountNumber { get; init; }

        [JsonPropertyName("code")]
        public string? Code { get; init; }

        [JsonPropertyName("content")]
        public string? Content { get; init; }

        [JsonPropertyName("transferType")]
        public string? TransferType { get; init; }

        [JsonPropertyName("transferAmount")]
        public decimal TransferAmount { get; init; }

        [JsonPropertyName("accumulated")]
        public decimal Accumulated { get; init; }

        [JsonPropertyName("subAccount")]
        public string? SubAccount { get; init; }

        [JsonPropertyName("referenceCode")]
        public string? ReferenceCode { get; init; }

        [JsonPropertyName("description")]
        public string? Description { get; init; }
    }
}