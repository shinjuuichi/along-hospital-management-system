namespace UploadSvc.Storage
{
    public interface IStorageService
    {
        Task UploadAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteAsync(string fileName);
    }
}
