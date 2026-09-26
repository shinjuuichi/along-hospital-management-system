using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace MedicineSvc.DAL.Models
{
    public class Option : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string OptionName { get; set; } = string.Empty;

        public virtual ICollection<MedicineUnitOption> MedicineUnitOptions { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<OptionValue> OptionValues { get; set; } = [];
    }
}