namespace SharedLibrary.DTOs.FileDTOs.Responses
{
    public sealed record UploadMultiFilesResponseDTO(
       UploadMultiFilesData data,
       object? error,
       string message
   );

    public sealed record UploadMultiFilesData(
        List<UploadFileData> successfulUploads,
        List<string> failedFiles,
        int successCount,
        int failureCount
    );
}
