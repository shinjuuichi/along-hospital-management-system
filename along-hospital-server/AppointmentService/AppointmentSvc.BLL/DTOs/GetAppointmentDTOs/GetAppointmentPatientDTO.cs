namespace AppointmentSvc.BLL.DTOs.GetAppointmentDTOs
{
    public class GetAppointmentPatientDTO : GetAppointmentUserDTO
    {
        public string? MedicalNumber { get; set; }

        public int? Height { get; set; }

        public int? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<GetAppointmentPatientAllergyDTO> Allergies { get; set; } = [];
    }

    public class GetAppointmentPatientAllergyDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? SeverityLevel { get; set; }

        public string? Reaction { get; set; }
    }
}