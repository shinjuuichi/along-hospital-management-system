using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAnnotations;

namespace PaymentSvc.DAL.Models
{
    [BsonDiscriminator("PayOS")]
    [CollectionName("Payments")]
    public class PayOSPayment : Payment
    {
        public long ProviderOrderCode { get; set; }
    }
}