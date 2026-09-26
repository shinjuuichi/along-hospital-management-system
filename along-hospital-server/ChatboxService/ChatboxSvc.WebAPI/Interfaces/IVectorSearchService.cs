using ChatboxSvc.WebAPI.DTOs;

namespace ChatboxSvc.WebAPI.Interfaces
{
    public interface IVectorSearchService
    {
        Task UpsertAsync(string id, float[] vector, string type, string payloadJson);
        Task<List<VectorSearchResultDTO>> SearchAsync(float[] vector, string type, int topK);
    }
}