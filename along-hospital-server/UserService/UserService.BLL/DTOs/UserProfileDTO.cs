using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;
using UserSvc.DAL.Models;

namespace UserSvc.BLL.DTOs
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(UserProfileDTO))]
    [JsonDerivedType(typeof(PatientProfileDTO))]
    [JsonDerivedType(typeof(DoctorProfileDTO))]
    public class UserProfileDTO : MapFrom<User>
    {
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }

    public class PatientProfileDTO : UserProfileDTO
    {
        public string? MedicalNumber { get; set; }
        public int Height { get; set; }
        public double Weight { get; set; }
        public string? BloodType { get; set; }
    }

    public class DoctorProfileDTO : UserProfileDTO
    {
        public string? QualificationName { get; set; }
        public string? SpecialtyName { get; set; }
    }
}
