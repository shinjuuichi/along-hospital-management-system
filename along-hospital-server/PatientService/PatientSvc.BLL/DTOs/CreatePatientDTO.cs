using AutoMapper;
using PatientSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PatientSvc.BLL.DTOs
{
    public class CreatePatientDTO : MapTo<Patient>
    {
        public int Id { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<UpsertAllergyDTO> Allergies { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePatientDTO, Patient>()
                .ForMember(d => d.MedicalNumber, opt => opt.MapFrom(s => $"PT-{DateTime.UtcNow.Year}-{s.Id:D6}"))
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }
}
