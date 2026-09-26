using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace InventorySvc.DAL.Models
{
    public class Inventory : AuditEntity
    {
        [MessageRequired, NumberPositive]
        public int Quantity { get; set; }

        public DateTime? LastImportDate { get; set; }

        [NumberPositive]
        public int? MinQuantity { get; set; }

        [NumberPositive, NumberHigherThanOrEqualTo(nameof(MinQuantity))]
        public int? MaxQuantity { get; set; }

        [MessageRequired]
        [MessageMaxLength(50)]
        public string SKUCode { get; set; } = string.Empty;
    }
}