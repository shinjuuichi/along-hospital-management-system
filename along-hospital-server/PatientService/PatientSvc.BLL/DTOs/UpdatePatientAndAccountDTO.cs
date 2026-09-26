namespace PatientSvc.BLL.DTOs
{
    public class UpdatePatientAndAccountDTO
    {
        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<UpsertAllergyDTO> Allergies { get; set; } = [];

        public string? Name { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }
    }
}