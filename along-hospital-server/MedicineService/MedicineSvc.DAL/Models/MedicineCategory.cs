using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicineSvc.DAL.Models
{
    public class MedicineCategory : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(255)]
        [Unique]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Medicine> Medicines { get; set; } = [];
    }
}