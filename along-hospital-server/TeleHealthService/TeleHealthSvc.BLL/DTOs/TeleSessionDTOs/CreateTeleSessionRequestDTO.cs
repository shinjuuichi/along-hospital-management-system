namespace TeleHealthSvc.BLL.DTOs.TeleSessionDTOs
{
    public class CreateTeleSessionRequestDTO
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int SpecialtyId { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
