using AppointmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace AppointmentSvc.BLL.DTOs.GetAppointmentDTOs
{
    public class GetAppointmentDTO : MapFrom<Appointment>
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string? Purpose { get; set; }

        public string? AppointmentStatus { get; set; }

        public string? AppointmentMeetingType { get; set; }

        public string? AppointmentPaymentStatus { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? CancelledDate { get; set; }

        public Guid? TransactionId { get; set; }

        public int? MedicalHistoryId { get; set; }

        public int SpecialtyId { get; set; }

        public GetAppointmentSpecialtyDTO? Specialty { get; set; }

        public int PatientId { get; set; }

        public GetAppointmentPatientDTO? Patient { get; set; }

        public int TimeSlotId { get; set; }

        public GetAppointmentTeleSessionDTO? TeleSession { get; set; }

        public GetAppointmentTimeSlotSnapshotDTO? TimeSlotSnapshot { get; set; }
    }
}
