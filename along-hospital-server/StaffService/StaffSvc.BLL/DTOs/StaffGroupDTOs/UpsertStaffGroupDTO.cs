using AutoMapper;
using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffGroupDTOs
{
    public class UpsertStaffGroupDTO : MapTo<StaffGroup>
    {
        public string Name { get; set; } = string.Empty;

        public List<int> StaffIds { get; set; } = [];

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<UpsertStaffGroupDTO, StaffGroup>()
                .ForMember(dest => dest.StaffGroupMembers,
                    opt => opt.MapFrom(src => src.StaffIds.Select(staffId => new StaffGroupMember
                    {
                        StaffId = staffId
                    }).ToList()));
        }
    }
}
