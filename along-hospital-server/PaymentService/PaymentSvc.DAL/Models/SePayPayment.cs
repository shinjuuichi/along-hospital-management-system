using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAnnotations;

namespace PaymentSvc.DAL.Models
{
    [BsonDiscriminator("SePay")]
    [CollectionName("Payments")]
    public class SePayPayment : Payment;
}