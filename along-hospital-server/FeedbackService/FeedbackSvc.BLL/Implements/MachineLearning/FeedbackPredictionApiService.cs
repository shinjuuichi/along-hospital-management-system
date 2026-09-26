using System.Text;
using System.Text.Json;
using FeedbackSvc.BLL.Interfaces.MachineLearning;
using Microsoft.Extensions.Logging;

namespace FeedbackSvc.BLL.Implements.MachineLearning
{
    public class FeedbackPredictionApiService(HttpClient httpClient, ILogger<FeedbackPredictionApiService> logger)
        : IFeedbackPredictionApiService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<FeedbackPredictionApiService> _logger = logger;

        private const string Endpoint = "/api/feedback";

        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<bool> GetFeedbackToxicPredictionAsync(string feedbackContent)
        {
            try
            {
                using var jsonContent = new StringContent(
                    JsonSerializer.Serialize(new { feedback = feedbackContent }),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(Endpoint + "/predict-toxic", jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var predictionResult = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, _jsonOptions);

                var predictionRaw = predictionResult?["prediction"]?.ToString();
                return bool.TryParse(predictionRaw, out var prediction) && prediction;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ML Service === : Error predicting feedback type: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetFeedbackTypePredictionAsync(string feedbackContent)
        {
            try
            {
                using var jsonContent = new StringContent(
                    JsonSerializer.Serialize(new { feedback = feedbackContent }),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(Endpoint + "/predict-sentiment", jsonContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var predictionResult = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, _jsonOptions);

                var predictionRaw = predictionResult?["prediction"]?.ToString();

                return predictionRaw ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task RetrainFeedbackTypeModelAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync(Endpoint + "/retrain-sentiment", null);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, _jsonOptions);
                var jsonResponseString = JsonSerializer.Serialize(jsonResponse, _jsonOptions);
                _logger.LogInformation("=== ML Service === : Feedback type model retrained successfully: {Response}", jsonResponseString);
            }
            catch (Exception ex)
            {
                _logger.LogError("=== ML Service === : Error retraining feedback type model: {ErrorMessage}", ex.Message);
            }
        }

        public async Task RetrainFeedbackToxicModelAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync(Endpoint + "/retrain-toxic", null);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent, _jsonOptions);
                var jsonResponseString = JsonSerializer.Serialize(jsonResponse, _jsonOptions);
                _logger.LogInformation("=== ML Service === : Toxic model retrained successfully: {Response}", jsonResponseString);
            }
            catch (Exception ex)
            {
                _logger.LogError("=== ML Service === : Error retraining toxic model: {ErrorMessage}", ex.Message);
            }
        }
    }
}