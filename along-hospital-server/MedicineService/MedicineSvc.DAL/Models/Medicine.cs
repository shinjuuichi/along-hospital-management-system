using MedicineSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace MedicineSvc.DAL.Models
{
    public class Medicine : EntityWithMultiImages
    {
        [MessageRequired]
        [MessageMaxLength(255)]
        [Unique]
        public string Name { get; set; } = string.Empty;

        [MessageRequired]
        [MessageMaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [MessageRequired]
        public int MedicineUnitId { get; set; }

        [MessageRequired]
        public int MedicineCategoryId { get; set; }

        public MedicineStatusEnum Status { get; set; } = MedicineStatusEnum.Draft;

        public bool IsPublic { get; set; } = false;

        public virtual MedicineUnit? MedicineUnit { get; set; }

        public virtual MedicineCategory? MedicineCategory { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<MedicineSKU> Skus { get; set; } = [];
    }
}