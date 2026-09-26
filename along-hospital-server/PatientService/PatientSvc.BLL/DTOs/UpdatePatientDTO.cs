using PatientSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PatientSvc.BLL.DTOs
{
    public class UpdatePatientDTO : MapTo<Patient>
    {
        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<UpsertAllergyDTO> Allergies { get; set; } = [];
    }
}
