using ChatboxSvc.WebAPI.DTOs;
using ChatboxSvc.WebAPI.Interfaces;
using System.Text.Json;

namespace ChatboxSvc.WebAPI.Implements
{
    public class PythonEmbeddingApiService(HttpClient httpClient) : IEmbeddingService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private const string Endpoint = "/api/vector-embedding/embed";

        public async Task<float[]> EmbedAsync(string text)
        {
            try
            {
                var requestBody = new { text };
                var response = await _httpClient.PostAsJsonAsync(Endpoint, requestBody);

                response.EnsureSuccessStatusCode();

                var embeddingResponse = await response.Content.ReadFromJsonAsync<EmbeddingResponseDTO>(_jsonOptions);
                return embeddingResponse?.Vector.ToArray() ?? [];
            }
            catch
            {
                return [];
            }
        }
    }
}