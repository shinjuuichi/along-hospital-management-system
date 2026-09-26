using SharedLibrary.Enums;

namespace UploadSvc.Processing
{
    public class DefaultFileProcessor : IFileProcessor
    {
        public bool CanHandle(FileType type) => true;

        public Task<Stream> ProcessAsync(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            return Task.FromResult(stream);
        }
    }
}
