using AutoMapper;
using MedicineSvc.BLL.DTOs.MedicineUnitOptionDTOs;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicineSvc.BLL.DTOs.MedicineUnitDTOs
{
    public class GetMedicineUnitDTO : MapFrom<MedicineUnit>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public List<GetMedicineUnitOptionDTO> Options { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<MedicineUnit, GetMedicineUnitDTO>()
                .ForMember(dest => dest.Options,
                    opt => opt.MapFrom(src => src.MedicineUnitOptions));
        }
    }
}
