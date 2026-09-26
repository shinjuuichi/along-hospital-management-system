using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.Mappers;
using StaffRequestSvc.BLL.DTOs;
using StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs;
using StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs;
using StaffRequestSvc.BLL.DTOs.StatisticsDTOs;
using StaffRequestSvc.DAL.Models;
using System.Reflection;

namespace StaffRequestSvc.BLL
{
    public class StaffRequestMappingProfile : BaseMappingProfile
    {
        public StaffRequestMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MappingStatistic();
            MappingLeaveRequest();
            MappingUser();
        }

        private void MappingStatistic()
        {
            CreateMap<StaffRequestStatisticsDTO, GetStaffRequestStatisticsByDateRangeContract>();
        }

        private void MappingLeaveRequest()
        {
            //Event to DTO
            CreateMap<GetApprovedLeaveByStaffsAndRangeEvent, GetApprovedLeavesByStaffsRangeDTO>();
            CreateMap<LeaveRequestApprovedEvent, LeaveRequestValidationDTO>();

            //DTO to Contract
            CreateMap<ApprovedLeaveDTO, ApprovedLeaveContract>();
            CreateMap<GetSalaryAdvanceDTO, GetUndisbursedSalaryAdvanceByStaffIdContract>();

            //DTO to DTO
            CreateMap<CreateLeaveRequestDTO, LeaveRequestValidationDTO>()
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom((_, _, _, context) =>
                    context.Items.TryGetValue(nameof(LeaveRequestValidationDTO.StaffId), out var staffId)
                        ? (int)staffId
                        : default))
                .ForMember(dest => dest.LeaveUnit, opt => opt.MapFrom(src => src.LeaveUnit ?? string.Empty));

            //Entity to DTO
            CreateMap<LeaveRequest, LeaveRequestValidationDTO>()
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.CreatedBy ?? default))
                .ForMember(dest => dest.LeaveUnit, opt => opt.MapFrom(src => src.LeaveUnit.ToString()));

            //DTO to Event
            CreateMap<LeaveRequestValidationDTO, ValidateLeaveRequestEvent>();
            CreateMap<LeaveRequestValidationDTO, LeaveRequestApprovedEvent>();
        }

        private void MappingUser()
        {
            CreateMap<GetUserDataByUserIdContract, UserInfoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
