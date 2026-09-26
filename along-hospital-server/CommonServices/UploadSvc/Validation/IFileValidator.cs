namespace UploadSvc.Validation
{
    public interface IFileValidator
    {
        Task<(bool IsValid, string? ErrorMessage)> ValidateAsync(IFormFile file);
    }
}
