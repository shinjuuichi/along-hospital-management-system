using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using UploadSvc.Services.Interfaces;

namespace UploadSvc.Controllers
{
    public class UploadController(IUploadService uploadService) : BaseController
    {
        private readonly IUploadService _uploadService = uploadService;

        [HttpPost]
        [RequestSizeLimit(30 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 30 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string? folder = null)
        {
            if (file == null)
            {
                return Result.FailError("No file provided", "File validation failed", 400);
            }

            var result = await _uploadService.UploadAsync(file, folder);
            return Result.SuccessData(result, "File uploaded successfully");
        }

        [HttpPost("multiple")]
        [RequestSizeLimit(300 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 300 * 1024 * 1024)]
        public async Task<IActionResult> UploadMultiple(IFormFileCollection files, [FromQuery] string? folder = null)
        {
            if (files == null || files.Count == 0)
            {
                return Result.FailError("No files provided", "File validation failed", 400);
            }

            if (files.Count > 10)
            {
                return Result.FailError("Maximum 10 files allowed per request", "File validation failed", 400);
            }

            var result = await _uploadService.UploadManyAsync(files, folder);
            return Result.SuccessData(result, $"Uploaded {result.SuccessCount} of {files.Count} files successfully");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result.FailError("No file name provided", "File name validation failed", 400);
            }

            await _uploadService.DeleteAsync(fileName);
            return Result.SuccessAction("File deleted successfully");
        }

        [HttpDelete("multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<string> fileNames)
        {
            if (fileNames == null || !fileNames.Any())
            {
                return Result.FailError("No file names provided", "File names validation failed", 400);
            }

            await _uploadService.DeleteManyAsync(fileNames);
            return Result.SuccessAction("All files deleted successfully.");
        }
    }
}
