using SharedLibrary.Enums;
using SharedLibrary.Utils;

namespace UploadSvc.Utils
{
    public static class FileTypeDetector
    {
        public static FileType Detect(string fileName)
        {
            return FileExtensionMap.DetectFileType(fileName);
        }
    }
}
