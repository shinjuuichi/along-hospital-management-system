using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicineSvc.DAL.Models
{
    public class MedicineUnit : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; } = [];

        public virtual ICollection<MedicineUnitOption> MedicineUnitOptions { get; set; } = [];
    }
}
