using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace VoucherSvc.DAL.Models
{
    [CollectionName("PatientVoucher")]
    public class PatientVoucher : MongoEntityWithImage
    {
        [MessageRequired]
        public int PatientId { get; set; }

        [MessageRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string VoucherId { get; set; } = string.Empty;

        public bool IsUsed { get; set; } = false;

        public DateTime? UsedAt { get; set; }
    }
}
