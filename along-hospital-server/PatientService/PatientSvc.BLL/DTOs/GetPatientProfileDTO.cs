using PatientSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PatientSvc.BLL.DTOs
{
    public class GetPatientProfileDTO : MapFrom<Patient>
    {
        public string? MedicalNumber { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }
    }
}
