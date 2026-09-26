namespace AppointmentSvc.BLL.DTOs.GetAppointmentDTOs
{
    public class GetAppointmentTeleSessionDTO
    {
        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string? PatientJoinUrl { get; set; }
    }
}
