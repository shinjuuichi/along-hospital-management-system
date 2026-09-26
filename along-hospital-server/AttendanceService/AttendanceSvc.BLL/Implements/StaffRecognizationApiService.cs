using AttendanceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AttendanceSvc.BLL.Implements
{
    public class StaffRecognizationApiService(HttpClient httpClient) : IStaffRecognizationApiService
    {
        private readonly HttpClient _httpClient = httpClient;

        private const string Endpoint = "api/staff-recognization";

        public async Task<int> RecognizeStaffAsync(IFormFile? file)
        {
            if (file == null)
            {
                return -1;
            }

            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync($"{Endpoint}/recognize", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var errorMessage = errorResponse?["error"] ?? "Unknown error occurred";
                throw new InvalidDataException(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            if (!int.TryParse(result?["staffId"], out var staffId))
            {
                return -1;
            }

            return staffId;
        }

        public async Task<bool> CheckIdentificationExistAsync(int staffId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Endpoint}/check-exist/{staffId}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, bool>>();
                return result?["exist"] ?? false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> EnrollAsync(int staffId, List<IFormFile> images)
        {
            using var content = new MultipartFormDataContent();
            foreach (var image in images)
            {
                var imageContent = new StreamContent(image.OpenReadStream());
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                content.Add(imageContent, "images", image.FileName);
            }

            var response = await _httpClient.PostAsync($"{Endpoint}/enroll?staffId={staffId}", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var errorMessage = errorResponse?["error"] ?? "Unknown error occurred";
                throw new InvalidDataException(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            var message = result?.TryGetValue("message", out var messageObj) == true
                ? messageObj?.ToString()
                : null;

            return string.IsNullOrWhiteSpace(message)
                ? "Valid face images accepted. Enrollment is processing in background, please wait 5-10 minutes."
                : message;
        }

        public async Task<string> ResetIdentificationAsync(int staffId)
        {
            var response = await _httpClient.DeleteAsync($"{Endpoint}/reset-identification/{staffId}");
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var errorMessage = errorResponse?["error"] ?? "Unknown error occurred";
                throw new InvalidDataException(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            var message = result?.TryGetValue("message", out var messageObj) == true
                ? messageObj?.ToString()
                : null;

            return string.IsNullOrWhiteSpace(message)
                ? "Identification reset successfully. You can enroll again."
                : message;
        }
    }
}