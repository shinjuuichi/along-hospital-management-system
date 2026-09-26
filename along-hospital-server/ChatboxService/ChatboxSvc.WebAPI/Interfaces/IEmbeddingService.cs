namespace ChatboxSvc.WebAPI.Interfaces
{
    public interface IEmbeddingService
    {
        Task<float[]> EmbedAsync(string text);
    }
}
