using SharedLibrary.Commons.EntityAbstractions;

namespace MedicalOrderSvc.DAL.Models.Snapshots
{
    public class MedicalServiceSnapshot : Entity
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Code { get; set; }
    }
}
