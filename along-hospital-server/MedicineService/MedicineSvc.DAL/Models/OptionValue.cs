using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicineSvc.DAL.Models
{
    public class OptionValue : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string ValueName { get; set; } = string.Empty;

        [NumberPositive]
        public int UnitMultiplier { get; set; }

        public bool IsActive { get; set; } = true;

        public int OptionId { get; set; }

        public virtual Option? Option { get; set; }
        public virtual ICollection<SKUValue> SkuValues { get; set; } = [];
    }
}