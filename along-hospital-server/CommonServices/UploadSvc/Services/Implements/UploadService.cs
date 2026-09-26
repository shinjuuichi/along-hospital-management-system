using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using UploadSvc.DTOs;
using UploadSvc.Processing;
using UploadSvc.Services.Interfaces;
using UploadSvc.Storage;
using UploadSvc.Utils;
using UploadSvc.Validation;

namespace UploadSvc.Services.Implements
{
    public class UploadService(
        IEnumerable<IFileProcessor> processors,
        IStorageService storageService,
        IFileValidator fileValidator) : IUploadService
    {
        private readonly IEnumerable<IFileProcessor> _processors = processors;
        private readonly IStorageService _storageService = storageService;
        private readonly IFileValidator _fileValidator = fileValidator;

        public async Task<UploadResultDto> UploadAsync(IFormFile file, string? folder = null)
        {
            var validationResult = await _fileValidator.ValidateAsync(file);
            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.ErrorMessage ?? "Unknown validation error";
                throw new ValidationFailureException([errorMessage]);
            }

            var fileType = FileTypeDetector.Detect(file.FileName);
            var processor = _processors.FirstOrDefault(p => p.CanHandle(fileType))
                ?? throw new InvalidOperationException($"No file processor available for {fileType}");

            using var processedStream = await processor.ProcessAsync(file);
            var fileName = GenerateFileName(file.FileName, fileType, folder);
            var contentType = ResolveContentType(fileType, file.ContentType);
            var fileSizeBytes = processedStream.CanSeek ? processedStream.Length : file.Length;

            await _storageService.UploadAsync(processedStream, fileName, contentType);

            return new UploadResultDto
            {
                FileName = fileName,
                PublicUrl = fileName,
                FileSizeBytes = fileSizeBytes
            };
        }

        public async Task<MultipleUploadResultDto> UploadManyAsync(IFormFileCollection files, string? folder = null)
        {
            var results = new List<UploadResultDto>();
            var failedFiles = new List<string>();

            foreach (var file in files)
            {
                var validationResult = await _fileValidator.ValidateAsync(file);
                if (!validationResult.IsValid)
                {
                    failedFiles.Add($"{file.FileName}: {validationResult.ErrorMessage}");
                    continue;
                }

                var fileType = FileTypeDetector.Detect(file.FileName);
                var processor = _processors.FirstOrDefault(p => p.CanHandle(fileType));
                if (processor is null)
                {
                    failedFiles.Add($"{file.FileName}: No processor available for {fileType}");
                    continue;
                }

                try
                {
                    using var processedStream = await processor.ProcessAsync(file);
                    var fileName = GenerateFileName(file.FileName, fileType, folder);
                    var contentType = ResolveContentType(fileType, file.ContentType);
                    var fileSizeBytes = processedStream.CanSeek ? processedStream.Length : file.Length;

                    await _storageService.UploadAsync(processedStream, fileName, contentType);

                    results.Add(new UploadResultDto
                    {
                        FileName = fileName,
                        PublicUrl = fileName,
                        FileSizeBytes = fileSizeBytes
                    });
                }
                catch (Exception ex)
                {
                    failedFiles.Add($"{file.FileName}: Upload failed - {ex.Message}");
                }
            }

            return new MultipleUploadResultDto
            {
                SuccessfulUploads = results,
                FailedFiles = failedFiles,
                SuccessCount = results.Count,
                FailureCount = failedFiles.Count
            };
        }

        public async Task DeleteAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ValidationFailureException(["File name is required"]);
            }

            var objectKey = NormalizeFileKey(fileName);
            await _storageService.DeleteAsync(objectKey);
        }

        public async Task DeleteManyAsync(IEnumerable<string> fileNames)
        {
            var failedDeletions = new List<string>();
            foreach (var fileName in fileNames)
            {
                try
                {
                    var objectKey = NormalizeFileKey(fileName);
                    if (string.IsNullOrWhiteSpace(objectKey))
                    {
                        continue;
                    }

                    await _storageService.DeleteAsync(objectKey);
                }
                catch
                {
                    failedDeletions.Add(fileName);
                }
            }

            if (failedDeletions.Count != 0)
            {
                throw new Exception($"Failed to delete {failedDeletions.Count} files.");
            }
        }

        private static string ResolveContentType(FileType fileType, string? originalContentType)
        {
            if (fileType == FileType.Image)
            {
                return "image/webp";
            }

            return string.IsNullOrWhiteSpace(originalContentType)
                ? "application/octet-stream"
                : originalContentType;
        }

        private static string GenerateFileName(string originalName, FileType fileType, string? folder)
        {
            var id = Guid.NewGuid().ToString("N");
            var extension = fileType == FileType.Image
                ? ".webp"
                : Path.GetExtension(originalName).ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".bin";
            }

            var finalFileName = $"{id}{extension}";
            var normalizedFolder = NormalizeFolder(folder);

            return string.IsNullOrWhiteSpace(normalizedFolder)
                ? finalFileName
                : $"{normalizedFolder}/{finalFileName}";
        }

        private static string NormalizeFolder(string? folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return string.Empty;
            }

            var sanitizedSegments = folder
                .Replace('\\', '/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Where(segment => segment != "." && segment != "..")
                .Select(segment => new string(segment.Where(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_').ToArray()))
                .Where(segment => !string.IsNullOrWhiteSpace(segment));

            return string.Join('/', sanitizedSegments);
        }

        private static string NormalizeFileKey(string fileName)
        {
            if (Uri.TryCreate(fileName, UriKind.Absolute, out var uri) && uri.IsAbsoluteUri)
            {
                return uri.AbsolutePath.TrimStart('/');
            }

            return fileName.Trim().TrimStart('/');
        }
    }
}
