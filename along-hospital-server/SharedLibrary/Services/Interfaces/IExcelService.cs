namespace SharedLibrary.Services.Interfaces
{
    public interface IExcelService
    {
        Task<List<Dictionary<string, object>>> ReadExcelToDictionaryAsync(Stream fileStream, string fileName);
        Task<List<T>> ReadExcelAsync<T>(Stream fileStream, string fileName) where T : class, new();
        Task<byte[]> WriteCsvAsync<T>(T data, string title) where T : class;
        Task<byte[]> WriteSectionedExcelAsync<T>(T data, string title, string? subtitle = null) where T : class;
    }
}