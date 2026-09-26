namespace SharedLibrary.DTOs.FileDTOs.Responses
{
    public sealed record UploadFileResponseDTO(
        UploadFileData data,
        object? error,
        string message
    );

    public sealed record UploadFileData(
        string fileName,
        string publicUrl,
        long fileSizeBytes
    );
}
