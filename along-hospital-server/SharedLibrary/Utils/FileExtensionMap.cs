using SharedLibrary.Enums;

namespace SharedLibrary.Utils
{
    public static class FileExtensionMap
    {
        public static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp", ".tiff"
        };

        public static readonly HashSet<string> DocumentExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".csv", ".zip", ".rar"
        };

        public static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".mov", ".avi", ".mkv", ".webm"
        };

        public static HashSet<string> GetAllowedExtensions(FileType[] fileTypes, string[]? customExtensions = null)
        {
            var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var fileType in fileTypes)
            {
                switch (fileType)
                {
                    case FileType.Image:
                        extensions.UnionWith(ImageExtensions);
                        break;
                    case FileType.Document:
                        extensions.UnionWith(DocumentExtensions);
                        break;
                    case FileType.Video:
                        extensions.UnionWith(VideoExtensions);
                        break;
                    case FileType.Other:
                        extensions.UnionWith(ImageExtensions);
                        extensions.UnionWith(DocumentExtensions);
                        extensions.UnionWith(VideoExtensions);
                        break;
                    case FileType.Custom:
                        break;
                }
            }

            if (customExtensions != null)
            {
                foreach (var ext in customExtensions)
                {
                    if (!string.IsNullOrWhiteSpace(ext))
                    {
                        var normalized = ext.Trim();
                        extensions.Add(normalized.StartsWith('.') ? normalized : $".{normalized}");
                    }
                }
            }

            return extensions;
        }

        public static FileType DetectFileType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            if (ImageExtensions.Contains(extension))
            {
                return FileType.Image;
            }

            if (DocumentExtensions.Contains(extension))
            {
                return FileType.Document;
            }

            if (VideoExtensions.Contains(extension))
            {
                return FileType.Video;
            }

            return FileType.Other;
        }
    }
}
