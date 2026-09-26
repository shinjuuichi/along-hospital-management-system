using MedicalHistorySvc.BLL.DTOs.ComplaintDTOs;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs;
using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.GetDTOs;
using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs
{
    public class GetMedicalHistoryDTO : MapFrom<MedicalHistory>
    {
        public int Id { get; set; }

        public string? MedicalHistoryNumber { get; set; }

        public string? Diagnosis { get; set; }

        public DateOnly? FollowUpAppointmentDate { get; set; }

        public string? MedicalHistoryStatus { get; set; }

        public string? MedicalHistoryType { get; set; }

        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public int PatientId { get; set; }

        public int? DoctorId { get; set; }

        public int SpecialtyId { get; set; }

        public GetMedicalHistoryPatientDTO? Patient { get; set; }

        public GetMedicalHistoryStaffDTO? Doctor { get; set; }

        public GetMedicalHistorySpecialtyDTO? Specialty { get; set; }

        public GetPrescriptionDTO? Prescription { get; set; }

        public GetComplaintDTO? Complaint { get; set; }

        public GetMedicalHistoryBedOccupancyDTO? BedOccupancy { get; set; }

        public List<GetMedicalHistoryBedOccupancyDTO> BedOccupancies { get; set; } = [];

        public List<GetMedicalHistoryInvoiceDTO> Invoices { get; set; } = [];

        public List<GetMedicalHistoryMedicalOrderDTO> MedicalOrders { get; set; } = [];
    }
}