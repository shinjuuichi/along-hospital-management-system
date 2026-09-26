using AutoMapper;
using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.UpsertDTOs
{
    public class UpsertPrescriptionDTO : MapTo<Prescription>
    {
        public string? DoctorNote { get; set; }

        public int MedicationDays { get; set; }

        public List<UpsertPrescriptionDetailDTO> PrescriptionDetails { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertPrescriptionDTO, Prescription>()
                .ForMember(dest => dest.PrescriptionDetails, opt => opt.Ignore())
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }
}