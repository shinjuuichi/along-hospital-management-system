using Microsoft.AspNetCore.Http;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Enums;

namespace PatientSvc.BLL.DTOs
{
    public class CreatePatientAndAccountDTO
    {
        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<UpsertAllergyDTO> Allergies { get; set; } = [];

        public string? Name { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFile? Image { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }
    }
}
