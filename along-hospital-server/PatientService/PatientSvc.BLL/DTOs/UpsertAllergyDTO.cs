using PatientSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PatientSvc.BLL.DTOs
{
    public class UpsertAllergyDTO : MapTo<Allergy>
    {
        public string? Name { get; set; }

        public string? SeverityLevel { get; set; }

        public string? Reaction { get; set; }
    }
}
