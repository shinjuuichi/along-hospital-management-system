using MedicalHistorySvc.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace MedicalHistorySvc.BLL.Implements
{
    public class ComplaintPredictionApiService(HttpClient httpClient, ILogger<ComplaintPredictionApiService> logger) : IComplaintPredictionApiService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<ComplaintPredictionApiService> _logger = logger;

        private const string Endpoint = "/api/complaint";

        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<string> GetComplaintTypePredictionAsync(string complaintText)
        {
            try
            {
                using var jsonContent = new StringContent(
                    JsonSerializer.Serialize(new { complaint = complaintText }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync(Endpoint + "/predict", jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var predictionResult = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent, _jsonOptions);

                return predictionResult?["prediction"] ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task RetrainComplaintTypeModelAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync(Endpoint + "/retrain", null);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, _jsonOptions);
                var jsonResponseString = JsonSerializer.Serialize(jsonResponse, _jsonOptions);
                _logger.LogInformation("=== ML Service === : Complaint type model retrained successfully: {Response}", jsonResponseString);
            }
            catch (Exception ex)
            {
                _logger.LogError("=== ML Service === : Error retraining complaint type model: {Message}", ex.Message);
            }
        }
    }
}
