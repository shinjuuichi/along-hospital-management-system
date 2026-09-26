using AutoMapper;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.BLL.DTOs.RoomCategoryDTOs
{
    public class GetRoomCategoryDTO : MapFrom<RoomCategory>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string> Roles { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<RoomCategory, GetRoomCategoryDTO>()
                .ForMember(d => d.Roles,
                    opt => opt.MapFrom(s => s.RoomCategoryRoleMappings
                        .Select(m => m.RoomCategoryRole!.Role.ToString()).ToList()));
        }
    }
}
