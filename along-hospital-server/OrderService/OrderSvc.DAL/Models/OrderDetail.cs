using Microsoft.EntityFrameworkCore;
using OrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace OrderSvc.DAL.Models
{
    [PrimaryKey(nameof(OrderId), nameof(SKUCode))]
    public class OrderDetail : Entity
    {
        public int OrderId { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        public string SKUCode { get; set; } = string.Empty;

        [NumberHigherThanOrEqualTo(1)]
        public int Quantity { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double UnitPrice { get; set; }

        [NumberHigherThanOrEqualTo(0)]
        public double? DiscountAmount { get; set; }

        [JsonColumn]
        public MedicineSnapshot? MedicineSnapshot { get; set; }

        public virtual Order? Order { get; set; }
    }
}
