using AutoMapper;
using MedicalHistorySvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs
{
    public class GetMedicalHistoryPatientDTO : GetMedicalHistoryUserDTO, IMapFrom<PatientSnapshot>
    {
        public string? MedicalNumber { get; set; }

        public int? Height { get; set; }

        public double? Weight { get; set; }

        public string? BloodType { get; set; }

        public List<GetMedicalHistoryPatientAllergyDTO> Allergies { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PatientSnapshot, GetMedicalHistoryPatientDTO>();
        }
    }

    public class GetMedicalHistoryPatientAllergyDTO : MapFrom<AllergySnapshot>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? SeverityLevel { get; set; }

        public string? Reaction { get; set; }
    }
}