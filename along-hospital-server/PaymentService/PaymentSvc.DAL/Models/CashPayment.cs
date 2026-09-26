using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAnnotations;

namespace PaymentSvc.DAL.Models
{
    [BsonDiscriminator("Cash")]
    [CollectionName("Payments")]
    public class CashPayment : Payment;
}