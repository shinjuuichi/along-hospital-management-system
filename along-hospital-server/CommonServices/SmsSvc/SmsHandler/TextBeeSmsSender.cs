using SharedLibrary.Commons;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SmsSvc.SmsHandler
{
    public class TextBeeSmsSender(AppConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<TextBeeSmsSender> logger) : ISmsSender
    {
        public async Task SendAsync(string toPhone, string content, CancellationToken cancellationToken = default)
        {
            var client = httpClientFactory.CreateClient(nameof(TextBeeSmsSender));
            client.BaseAddress = new Uri(configuration.TextBeeConfig.BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Remove("x-api-key");
            client.DefaultRequestHeaders.Add("x-api-key", configuration.TextBeeConfig.ApiKey);

            var payload = new
            {
                recipients = new[] { toPhone },
                message = content
            };

            var json = JsonSerializer.Serialize(payload);
            using var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            var deviceId = configuration.TextBeeConfig.DeviceId;
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                logger.LogError("TextBeeConfig.DeviceId is missing");
                throw new InvalidOperationException("TextBeeConfig.DeviceId is required for sending SMS.");
            }

            var endpoint = $"gateway/devices/{deviceId}/send-sms";
            var baseUrl = configuration.TextBeeConfig.BaseUrl?.TrimEnd('/') ?? "https://api.textbee.dev/api/v1";
            var fullUrl = $"{baseUrl}/{endpoint}";

            try
            {
                var response = await client.PostAsync(fullUrl, contentJson, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);
                    logger.LogError("Failed to send SMS to {Phone}. Status: {StatusCode}, Reason: {Reason}, Body: {Body}",
                        toPhone, (int)response.StatusCode, response.ReasonPhrase, body);
                    throw new HttpRequestException($"TextBee send failed: {(int)response.StatusCode} {response.ReasonPhrase} - {body}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred while sending SMS to {Phone}", toPhone);
                throw;
            }
        }
    }
}
