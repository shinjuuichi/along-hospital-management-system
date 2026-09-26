using AutoMapper;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BedDTOs
{
    public class GetBedDTO : MapFrom<Bed>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int RoomId { get; set; }
        public int BedCategoryId { get; set; }
        public string? BedCategoryCode { get; set; }
        public string? BedCategoryName { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Bed, GetBedDTO>()
                .ForMember(d => d.BedCategoryCode, opt => opt.MapFrom(s => s.BedCategory!.Code))
                .ForMember(d => d.BedCategoryName, opt => opt.MapFrom(s => s.BedCategory!.Name));
        }
    }
}