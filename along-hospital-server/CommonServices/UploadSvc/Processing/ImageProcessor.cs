using SharedLibrary.Enums;
using SixLabors.ImageSharp.Formats.Webp;

namespace UploadSvc.Processing
{
    public class ImageProcessor : IFileProcessor
    {
        public bool CanHandle(FileType type) => type == FileType.Image;

        public async Task<Stream> ProcessAsync(IFormFile file)
        {
            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            var outputStream = new MemoryStream();

            await image.SaveAsWebpAsync(outputStream, new WebpEncoder
            {
                Quality = 75
            });

            outputStream.Position = 0;
            return outputStream;
        }
    }
}
