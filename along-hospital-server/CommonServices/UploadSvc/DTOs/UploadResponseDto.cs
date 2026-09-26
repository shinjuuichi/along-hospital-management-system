namespace UploadSvc.DTOs
{
    public class UploadResultDto
    {
        public string FileName { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
    }

    public class MultipleUploadResultDto
    {
        public List<UploadResultDto> SuccessfulUploads { get; set; } = new();
        public List<string> FailedFiles { get; set; } = new();
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }
}