using System.Text.Json.Serialization;

namespace PaymentSvc.BLL.DTOs.ExchangeRateDTOs
{
    public record ExchangeRateRespondDTO
    {
        public string? Result { get; init; }

        [JsonPropertyName("conversion_rate")]
        public decimal ConversionRate { get; init; }
    }
}