using MongoDB.Bson.Serialization.Attributes;

namespace SharedLibrary.Commons.EntityAbstractions.Mongo
{
    public abstract class MongoAuditEntity : MongoBaseEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public int? CreatedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ModificationDate { get; set; }

        public int? ModifiedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? DeletionDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}