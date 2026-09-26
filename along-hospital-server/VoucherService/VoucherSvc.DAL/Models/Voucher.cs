using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using VoucherSvc.DAL.Enums;

namespace VoucherSvc.DAL.Models
{
    [CollectionName("Voucher")]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(MedicineDiscount))]
    public class Voucher : MongoEntityWithImage
    {
        [MessageRequired, MessageMaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string? Description { get; set; }

        [Unique, MessageMaxLength(50)]
        public string Code { get; set; } = string.Empty;

        public VoucherStatusEnum VoucherStatus { get; set; } = VoucherStatusEnum.Active;

        [DateValidator(AllowPast = false)]
        public DateOnly ExpireDate { get; set; }

        [NumberPositive]
        public int Quantity { get; set; }

        public DiscountTypeEnum DiscountType { get; set; } = DiscountTypeEnum.Percentage;

        [NumberPositive]
        public double DiscountValue { get; set; }

        [NumberPositive]
        public double MaxDiscount { get; set; }

        [NumberPositive]
        public double MinPurchaseAmount { get; set; }

        public VoucherTypeEnum VoucherType { get; set; } = VoucherTypeEnum.Patient;
    }
}