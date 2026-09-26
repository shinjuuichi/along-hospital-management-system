using System.Reflection;
using AttendanceSvc.BLL.DTOs;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AttendanceEvents;
using MessageBroker.Contracts.AttendanceContracts;
using SharedLibrary.Base.Mappers;

namespace AttendanceSvc.BLL
{
    public class AttendanceMappingProfile : BaseMappingProfile
    {
        public AttendanceMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<GetStaffDataByUserIdContract, GetAttendanceStaffDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));
            CreateMap<GetAttendanceByStaffsAndRangeEvent, GetAttendanceLogsByStaffsRangeDTO>();
            CreateMap<GetAttendanceLogDTO, AttendanceLogContract>();
        }
    }
}
