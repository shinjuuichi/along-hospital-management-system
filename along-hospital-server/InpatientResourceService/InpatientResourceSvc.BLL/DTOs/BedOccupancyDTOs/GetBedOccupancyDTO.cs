using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.RoomDTOs;
using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs
{
    public class GetBedOccupancyDTO : MapFrom<BedOccupancy>
    {
        public int Id { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public double DurationInDays { get; set; }
        public double UnitPrice { get; set; }
        public double TotalAmount { get; set; }
        public string? OccupancyStatus { get; set; }
        public string? TransferNote { get; set; }
        public string? LatestTransferNote { get; set; }
        public int MedicalHistoryId { get; set; }
        public int BedId { get; set; }
        public GetBedOccupancyDetailDTO? Bed { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<BedOccupancy, GetBedOccupancyDTO>()
                .ForMember(d => d.Bed, opt => opt.MapFrom(s => s.Bed));
        }
    }

    public class GetBedOccupancyDetailDTO : MapFrom<Bed>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int BedCategoryId { get; set; }
        public string? BedCategoryCode { get; set; }
        public string? BedCategoryName { get; set; }
        public GetRoomDTO? Room { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Bed, GetBedOccupancyDetailDTO>()
                .ForMember(d => d.BedCategoryCode, opt => opt.MapFrom(s => s.BedCategory!.Code))
                .ForMember(d => d.BedCategoryName, opt => opt.MapFrom(s => s.BedCategory!.Name))
                .ForMember(d => d.Room, opt => opt.MapFrom(s => s.Room == null
                    ? null
                    : new GetRoomDTO
                    {
                        Id = s.Room.Id,
                        Code = s.Room.Code,
                        Status = s.Room.Status.ToString(),
                        SpecialtyId = s.Room.SpecialtyId,
                        FloorId = s.Room.FloorId,
                        FloorNumber = s.Room.Floor != null ? s.Room.Floor.FloorNumber : 0,
                        BuildingId = s.Room.Floor != null ? s.Room.Floor.BuildingId : 0,
                        BuildingName = s.Room.Floor != null && s.Room.Floor.Building != null
                            ? s.Room.Floor.Building.Name
                            : null,
                    }));
        }
    }
}