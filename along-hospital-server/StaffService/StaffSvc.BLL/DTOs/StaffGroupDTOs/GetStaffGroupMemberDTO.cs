using SharedLibrary.Base.Mappers;
using StaffSvc.BLL.DTOs.StaffAccountDTOs;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffGroupDTOs
{
    public class GetStaffGroupMemberDTO : MapFrom<StaffGroupMember>
    {
        public int StaffGroupId { get; set; }

        public int StaffId { get; set; }

        public GetStaffAndAccountDTO? Staff { get; set; }
    }
}