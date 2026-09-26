namespace TeleHealthSvc.BLL.DTOs.TeleSessionDTOs
{
    public class GetTeleSessionDTO
    {
        public int AppointmentId { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string? PatientJoinUrl { get; set; }
    }
}
