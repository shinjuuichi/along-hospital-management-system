using MongoDB.Bson.Serialization.Attributes;

namespace SharedLibrary.Commons.EntityAbstractions.Mongo
{
    public abstract class MongoBaseEntity : Entity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
    }
}