using PatientSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PatientSvc.BLL.DTOs
{
    public class GetPatientDTO : MapFrom<Patient>
    {
        public int Id { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? MedicalNumber { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<GetPatientAllergyDTO> Allergies { get; set; } = [];
    }
}