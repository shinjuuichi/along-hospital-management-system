using ChatboxSvc.WebAPI.DTOs;
using ChatboxSvc.WebAPI.Interfaces;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using SharedLibrary.Commons;
using System.Security.Cryptography;

namespace ChatboxSvc.WebAPI.Implements
{
    public class QdrantVectorSearchService(QdrantClient qdrantClient, AppConfiguration configuration) : IVectorSearchService
    {
        private readonly QdrantClient _qdrantClient = qdrantClient;
        private readonly string CollectionName = configuration.QdrantConfig.CollectionName;

        public async Task UpsertAsync(string id, float[] vector, string type, string payloadJson)
        {
            var point = new PointStruct
            {
                Id = new PointId
                {
                    Uuid = this.CreateGuidFromString(id).ToString()
                },
                Vectors = vector,
                Payload =
                {
                    ["type"] = type,
                    ["data"] = payloadJson
                }
            };

            await _qdrantClient.UpsertAsync(
                collectionName: CollectionName,
                points: [point]);
        }

        public async Task<List<VectorSearchResultDTO>> SearchAsync(float[] vector, string type, int topK)
        {
            var filter = new Filter
            {
                Must = {
                    new Condition {
                        Field = new FieldCondition {
                            Key = "type",
                            Match = new Match { Keyword = type }
                        }
                    }
                }
            };

            var results = await _qdrantClient.SearchAsync(
                collectionName: CollectionName,
                vector: vector,
                limit: (ulong)topK,
                filter: filter);

            return results.Select(r => new VectorSearchResultDTO
            {
                Score = r.Score,
                PayloadJson = r.Payload.TryGetValue("data", out var v)
                    ? v.StringValue
                    : string.Empty
            }).ToList();
        }

        private Guid CreateGuidFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Guid.Empty;
            }

            using var sha1 = SHA1.Create();

            var hashBytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
            var guidBytes = new byte[16];

            Array.Copy(hashBytes, guidBytes, 16);
            return new Guid(guidBytes);
        }
    }
}