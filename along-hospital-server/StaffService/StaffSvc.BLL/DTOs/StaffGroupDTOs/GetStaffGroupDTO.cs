using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffGroupDTOs
{
    public class GetStaffGroupDTO : MapFrom<StaffGroup>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<GetStaffGroupMemberDTO> StaffGroupMembers { get; set; } = [];

        public DateTime CreationDate { get; set; }
    }
}
