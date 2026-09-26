using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace MedicineSvc.DAL.Models
{
    public class MedicineSKU : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string SKUCode { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [NumberPositive]
        public double Price { get; set; }

        public bool IsActive { get; set; } = true;

        [MessageRequired]
        public int MedicineId { get; set; }

        public virtual Medicine? Medicine { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<SKUValue> SkuValues { get; set; } = [];
    }
}