using SharedLibrary.Commons.EntityAbstractions;

namespace AppointmentSvc.DAL.Models
{
    public class TimeSlotSnapshot : Entity
    {
        public int Id { get; set; }

        public TimeOnly Time { get; set; }
    }
}