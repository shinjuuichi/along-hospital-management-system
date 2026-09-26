using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;

namespace MedicineSvc.DAL.Models
{
    [PrimaryKey(nameof(MedicineUnitId), nameof(OptionId))]
    public class MedicineUnitOption : Entity
    {
        public int MedicineUnitId { get; set; }

        public int OptionId { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual MedicineUnit? MedicineUnit { get; set; }

        public virtual Option? Option { get; set; }
    }
}
