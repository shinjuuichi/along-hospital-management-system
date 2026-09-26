using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PaymentSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace PaymentSvc.DAL.Models
{
    [CollectionName("Payments")]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(PayOSPayment), typeof(CashPayment), typeof(SePayPayment))]
    public class Payment : MongoAuditEntity
    {
        [Unique]
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        [NumberHigherThanOrEqualTo(0)]
        public double OriginalAmount { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double? FinalAmount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? ExchangeRate { get; set; }

        public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Pending;

        [MessageRequired, MessageMaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [MessageRequired]
        public List<PaymentItem> PaymentItems { get; set; } = [];
    }

    public class PaymentItem
    {
        [MessageRequired, MessageMaxLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        [NumberHigherThan(0)]
        public int Quantity { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double UnitPrice { get; set; }
    }
}