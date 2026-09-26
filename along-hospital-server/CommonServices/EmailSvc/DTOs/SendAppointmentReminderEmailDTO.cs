namespace EmailSvc.DTOs
{
    public class SendAppointmentReminderEmailDTO
    {
        public string Email { get; set; } = default!;

        public int AppointmentId { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public string? SpecialtyName { get; set; }

        public string? PatientName { get; set; }
    }
}
