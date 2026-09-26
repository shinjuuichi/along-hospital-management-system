using Microsoft.AspNetCore.Http;

namespace SharedLibrary.Services.Interfaces
{
    public interface IUploadFileService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task<string[]> UploadManyAsync(IFormFileCollection files, string folder);
        Task DeleteAsync(string? file);
        Task DeleteManyAsync(string[] files);
    }
}
