using Microsoft.AspNetCore.Http;
using SharedLibrary.DTOs.FileDTOs.Responses;
using SharedLibrary.Services.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SharedLibrary.Services.Implements
{
    public class UploadFileService(IHttpClientFactory httpClientFactory) : IUploadFileService
    {
        private readonly HttpClient _uploadService = httpClientFactory.CreateClient("UploadService");

        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            file = file ?? throw new ArgumentNullException(nameof(file));

            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
            content.Add(streamContent, "file", file.FileName);

            var endpoint = BuildUploadEndpoint("api/v1/upload", folder);
            using var response = await _uploadService.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var upload = await response.Content.ReadFromJsonAsync<UploadFileResponseDTO>()
                ?? throw new InvalidDataException("Upload response is null");

            return upload.data.publicUrl;
        }

        public async Task<string[]> UploadManyAsync(IFormFileCollection files, string folder)
        {
            if (files == null || files.Count == 0)
            {
                return [];
            }

            using var content = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                content.Add(streamContent, "files", file.FileName);
            }

            var endpoint = BuildUploadEndpoint("api/v1/upload/multiple", folder);
            using var response = await _uploadService.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            var upload = await response.Content.ReadFromJsonAsync<UploadMultiFilesResponseDTO>()
                ?? throw new InvalidDataException("Upload response is null");

            return upload.data.successfulUploads
                .Select(file => file.publicUrl)
                .ToArray();
        }

        public async Task DeleteAsync(string? file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return;
            }

            var fileKey = ExtractFileKey(file);
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return;
            }

            var endpoint = $"api/v1/upload?fileName={Uri.EscapeDataString(fileKey)}";
            using var response = await _uploadService.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteManyAsync(string[] files)
        {
            if (files == null || files.Length == 0)
            {
                return;
            }

            var fileKeys = files
                .Where(file => !string.IsNullOrWhiteSpace(file))
                .Select(ExtractFileKey)
                .Where(fileKey => !string.IsNullOrWhiteSpace(fileKey))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (fileKeys.Length == 0)
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/upload/multiple")
            {
                Content = JsonContent.Create(fileKeys)
            };

            using var response = await _uploadService.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private static string BuildUploadEndpoint(string baseEndpoint, string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return baseEndpoint;
            }

            return $"{baseEndpoint}?folder={Uri.EscapeDataString(folder)}";
        }

        private static string ExtractFileKey(string file)
        {
            if (Uri.TryCreate(file, UriKind.Absolute, out var uri) && uri.IsAbsoluteUri)
            {
                return uri.AbsolutePath.TrimStart('/');
            }

            return file.Trim().TrimStart('/');
        }
    }
}
