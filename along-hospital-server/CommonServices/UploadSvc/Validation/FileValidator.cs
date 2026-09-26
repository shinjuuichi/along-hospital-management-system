using System.Text;

namespace UploadSvc.Validation
{
    public class FileValidator : IFileValidator
    {
        private const long MaxFileSizeBytes = 30 * 1024 * 1024;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp", ".tiff",
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".csv", ".zip", ".rar",
            ".mp4", ".mov", ".avi", ".mkv", ".webm"
        };

        private static readonly Dictionary<string, HashSet<string>> MimeTypesByExtension = new(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/jpeg" },
            [".jpeg"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/jpeg" },
            [".png"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/png" },
            [".webp"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/webp" },
            [".gif"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/gif" },
            [".bmp"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/bmp" },
            [".tiff"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "image/tiff", "image/tif" },
            [".pdf"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/pdf" },
            [".doc"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/msword" },
            [".docx"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            [".xls"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/vnd.ms-excel" },
            [".xlsx"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            [".ppt"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/vnd.ms-powerpoint" },
            [".pptx"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            [".txt"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "text/plain" },
            [".csv"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "text/csv", "application/csv", "application/vnd.ms-excel" },
            [".zip"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/zip", "application/x-zip-compressed" },
            [".rar"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "application/vnd.rar", "application/x-rar-compressed" },
            [".mp4"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "video/mp4" },
            [".mov"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "video/quicktime" },
            [".avi"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "video/x-msvideo" },
            [".mkv"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "video/x-matroska" },
            [".webm"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "video/webm" }
        };

        private static readonly byte[] JpegSignature = [0xFF, 0xD8, 0xFF];
        private static readonly byte[] PngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
        private static readonly byte[] Gif87aSignature = Encoding.ASCII.GetBytes("GIF87a");
        private static readonly byte[] Gif89aSignature = Encoding.ASCII.GetBytes("GIF89a");
        private static readonly byte[] BmpSignature = [0x42, 0x4D];
        private static readonly byte[] TiffLeSignature = [0x49, 0x49, 0x2A, 0x00];
        private static readonly byte[] TiffBeSignature = [0x4D, 0x4D, 0x00, 0x2A];
        private static readonly byte[] PdfSignature = Encoding.ASCII.GetBytes("%PDF-");
        private static readonly byte[] OleSignature = [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1];
        private static readonly byte[] ZipSignature1 = [0x50, 0x4B, 0x03, 0x04];
        private static readonly byte[] ZipSignature2 = [0x50, 0x4B, 0x05, 0x06];
        private static readonly byte[] ZipSignature3 = [0x50, 0x4B, 0x07, 0x08];
        private static readonly byte[] Rar4Signature = [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00];
        private static readonly byte[] Rar5Signature = [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00];
        private static readonly byte[] EbmlSignature = [0x1A, 0x45, 0xDF, 0xA3];
        private static readonly byte[] RiffSignature = Encoding.ASCII.GetBytes("RIFF");
        private static readonly byte[] AviSignature = Encoding.ASCII.GetBytes("AVI ");
        private static readonly byte[] WebpSignature = Encoding.ASCII.GetBytes("WEBP");
        private static readonly byte[] FtypSignature = Encoding.ASCII.GetBytes("ftyp");

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return (false, "File is empty or null");
            }

            if (file.Length > MaxFileSizeBytes)
            {
                return (false, $"File size exceeds {MaxFileSizeBytes / (1024 * 1024)} MB limit");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                return (false, $"File type {extension} is not supported");
            }

            if (!HasValidMimeType(extension, file.ContentType))
            {
                return (false, $"MIME type {file.ContentType} is not allowed for extension {extension}");
            }

            if (!await HasValidMagicNumberAsync(file, extension))
            {
                return (false, "File signature does not match file extension");
            }

            return (true, null);
        }

        private static bool HasValidMimeType(string extension, string? contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            var mimeType = contentType.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[0];

            if (mimeType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!MimeTypesByExtension.TryGetValue(extension, out var allowedMimeTypes))
            {
                return false;
            }

            return allowedMimeTypes.Contains(mimeType);
        }

        private static async Task<bool> HasValidMagicNumberAsync(IFormFile file, string extension)
        {
            if (extension is ".txt" or ".csv")
            {
                return true;
            }

            using var stream = file.OpenReadStream();
            var header = new byte[16];
            var bytesRead = await stream.ReadAsync(header);

            if (bytesRead == 0)
            {
                return false;
            }

            return extension switch
            {
                ".jpg" or ".jpeg" => StartsWith(header, bytesRead, JpegSignature),
                ".png" => StartsWith(header, bytesRead, PngSignature),
                ".gif" => StartsWith(header, bytesRead, Gif87aSignature) || StartsWith(header, bytesRead, Gif89aSignature),
                ".bmp" => StartsWith(header, bytesRead, BmpSignature),
                ".tiff" => StartsWith(header, bytesRead, TiffLeSignature) || StartsWith(header, bytesRead, TiffBeSignature),
                ".webp" => IsRiffType(header, bytesRead, WebpSignature),
                ".pdf" => StartsWith(header, bytesRead, PdfSignature),
                ".doc" or ".xls" or ".ppt" => StartsWith(header, bytesRead, OleSignature),
                ".docx" or ".xlsx" or ".pptx" or ".zip" => StartsWith(header, bytesRead, ZipSignature1) || StartsWith(header, bytesRead, ZipSignature2) || StartsWith(header, bytesRead, ZipSignature3),
                ".rar" => StartsWith(header, bytesRead, Rar4Signature) || StartsWith(header, bytesRead, Rar5Signature),
                ".avi" => IsRiffType(header, bytesRead, AviSignature),
                ".mp4" or ".mov" => IsIsoBaseMediaFile(header, bytesRead),
                ".mkv" or ".webm" => StartsWith(header, bytesRead, EbmlSignature),
                _ => false
            };
        }

        private static bool StartsWith(byte[] data, int length, byte[] signature)
        {
            if (length < signature.Length)
            {
                return false;
            }

            for (var i = 0; i < signature.Length; i++)
            {
                if (data[i] != signature[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsRiffType(byte[] header, int length, byte[] format)
        {
            if (length < 12)
            {
                return false;
            }

            return StartsWith(header, length, RiffSignature)
                && header[8] == format[0]
                && header[9] == format[1]
                && header[10] == format[2]
                && header[11] == format[3];
        }

        private static bool IsIsoBaseMediaFile(byte[] header, int length)
        {
            if (length < 12)
            {
                return false;
            }

            return header[4] == FtypSignature[0]
                && header[5] == FtypSignature[1]
                && header[6] == FtypSignature[2]
                && header[7] == FtypSignature[3];
        }
    }
}
