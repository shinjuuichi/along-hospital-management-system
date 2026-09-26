using SharedLibrary.Enums;

namespace UploadSvc.Processing
{
    public interface IFileProcessor
    {
        bool CanHandle(FileType type);

        Task<Stream> ProcessAsync(IFormFile file);
    }
}
