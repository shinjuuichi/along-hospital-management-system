using SharedLibrary.Commons.EntityAbstractions;

namespace QueueSvc.DAL.Models.Snapshots
{
    public class MedicalHistorySnapshot : Entity
    {
        public string? MedicalHistoryNumber { get; set; }

        public string? MedicalHistoryType { get; set; }

        public int PatientId { get; set; }
    }
}