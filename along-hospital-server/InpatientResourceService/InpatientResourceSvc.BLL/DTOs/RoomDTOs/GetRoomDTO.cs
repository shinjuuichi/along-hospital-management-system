using AutoMapper;
using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.RoomDTOs
{
    public class GetRoomDTO : MapFrom<Room>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int BuildingId { get; set; }
        public string? BuildingName { get; set; }
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
        public int RoomCategoryId { get; set; }
        public string? RoomCategoryName { get; set; }
        public int SpecialtyId { get; set; }
        public string? SpecialtyName { get; set; }
        public int BedCount { get; set; }
        public int AvailableBedCount { get; set; }
        public List<string> Roles { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Room, GetRoomDTO>()
                .ForMember(d => d.Roles,
                             opt => opt.MapFrom(s => s.RoomCategory!.RoomCategoryRoleMappings
                                .Select(m => m.RoomCategoryRole!.Role.ToString()).ToList()))
                .ForMember(d => d.RoomCategoryName, opt => opt.MapFrom(s => s.RoomCategory!.Name))
                .ForMember(d => d.FloorNumber, opt => opt.MapFrom(s => s.Floor!.FloorNumber))
                .ForMember(d => d.BuildingId, opt => opt.MapFrom(s => s.Floor!.BuildingId))
                .ForMember(d => d.BuildingName, opt => opt.MapFrom(s => s.Floor!.Building!.Name))
                .ForMember(d => d.BedCount, opt => opt.MapFrom(s => s.Beds != null ? s.Beds.Count : 0))
                .ForMember(d => d.AvailableBedCount, opt => opt.MapFrom(s => s.Beds != null ? s.Beds.Count(b => b.Status == BedStatusEnum.Active) : 0));
        }
    }
}
