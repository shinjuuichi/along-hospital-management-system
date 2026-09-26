using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAnnotations;

namespace VoucherSvc.DAL.Models
{
    [BsonDiscriminator("MedicineVoucher")]
    [CollectionName("Voucher")]
    public class MedicineDiscount : Voucher
    {
        public List<int> MedicineIds { get; set; } = [];
    }
}
