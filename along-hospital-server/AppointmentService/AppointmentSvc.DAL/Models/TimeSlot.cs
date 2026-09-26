using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace AppointmentSvc.DAL.Models
{
    public class TimeSlot : AuditEntity
    {
        [Unique]
        public TimeOnly Time { get; set; }

        [NumberHigherThanOrEqualTo(1)]
        public int CapacityPerDoctor { get; set; }

        [OnDelete(OnDeleteBehavior.NoAction)]
        public virtual ICollection<Appointment> Appointments { get; set; } = [];
    }
}