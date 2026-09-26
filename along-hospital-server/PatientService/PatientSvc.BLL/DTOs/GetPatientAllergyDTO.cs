using PatientSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PatientSvc.BLL.DTOs
{
    public class GetPatientAllergyDTO : MapFrom<Allergy>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? SeverityLevel { get; set; }

        public string? Reaction { get; set; }
    }
}
