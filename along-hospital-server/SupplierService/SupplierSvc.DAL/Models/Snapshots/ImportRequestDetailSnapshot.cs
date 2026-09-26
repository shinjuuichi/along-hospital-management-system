using SharedLibrary.Commons.EntityAbstractions;

namespace SupplierSvc.DAL.Models.Snapshots
{
    public class ImportRequestDetailSnapshot : Entity
    {
        public string? MedicineName { get; set; }

        public double? UnitPrice { get; set; }
    }
}
