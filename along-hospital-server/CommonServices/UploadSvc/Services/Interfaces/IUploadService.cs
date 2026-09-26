using UploadSvc.DTOs;

namespace UploadSvc.Services.Interfaces
{
    public interface IUploadService
    {
        Task<UploadResultDto> UploadAsync(IFormFile file, string? folder = null);
        Task<MultipleUploadResultDto> UploadManyAsync(IFormFileCollection files, string? folder = null);
        Task DeleteAsync(string fileName);
        Task DeleteManyAsync(IEnumerable<string> fileNames);
    }
}
