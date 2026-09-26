using SharedLibrary.Commons.EntityAbstractions;

namespace OrderSvc.DAL.Models.Snapshots
{
    public class MedicineSnapshot : Entity
    {
        public string? MedicineName { get; set; }

        public string? MedicineBrand { get; set; }

        public string[] MedicineImages { get; set; } = [];

        public string? MedicineUnit { get; set; }
    }
}