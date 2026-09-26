namespace AppointmentSvc.BLL.DTOs.TimeSlotDTOs
{
    public class ValidateTimeSlotDTO
    {
        public int TimeSlotId { get; set; }

        public DateOnly Date { get; set; }

        public int SpecialtyId { get; set; }

        public string? AppointmentMeetingType { get; set; }
    }
}