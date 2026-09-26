using SharedLibrary.Commons.EntityAbstractions;

namespace MedicalOrderSvc.DAL.Models.Snapshots
{
    public class MedicineSnapshot : Entity
    {
        public string? Name { get; set; }

        public string? Brand { get; set; }

        public string? MedicineUnit { get; set; }

        public string? MedicineImage { get; set; }

        public string? CategoryName { get; set; }
    }
}
