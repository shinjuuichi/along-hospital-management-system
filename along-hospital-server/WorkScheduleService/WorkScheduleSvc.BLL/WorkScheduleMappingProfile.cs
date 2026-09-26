using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.StaffRequestEvents;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.HolidayDTOs;
using WorkScheduleSvc.BLL.DTOs.ShiftDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL
{
    public class WorkScheduleMappingProfile : BaseMappingProfile
    {
        public WorkScheduleMappingProfile()
            : base(Assembly.GetExecutingAssembly())
        {
            MapClone();
            MapContract();
            MapHolidayRange();
        }

        private void MapClone()
        {
            CreateMap<WorkScheduleTemplate, WorkScheduleTemplate>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CreationDate, o => o.Ignore())
                .ForMember(d => d.IsActive, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.ModificationDate, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.DeletionDate, o => o.Ignore())
                .ForMember(d => d.IsDeleted, o => o.Ignore());

            CreateMap<WorkScheduleTemplateDayShift, WorkScheduleTemplateDayShift>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.WorkScheduleTemplateId, o => o.Ignore())
                .ForMember(d => d.WorkScheduleTemplate, o => o.Ignore())
                .ForMember(d => d.Shift, o => o.Ignore())
                .ForMember(d => d.CreationDate, o => o.Ignore())
                .ForMember(d => d.CreatedBy, o => o.Ignore())
                .ForMember(d => d.ModificationDate, o => o.Ignore())
                .ForMember(d => d.ModifiedBy, o => o.Ignore())
                .ForMember(d => d.DeletionDate, o => o.Ignore())
                .ForMember(d => d.IsDeleted, o => o.Ignore());

            CreateMap<WorkScheduleTemplateAssignmentForStaffRoom, WorkScheduleTemplateAssignmentForStaffRoom>()
                .ForMember(d => d.WorkScheduleTemplateId, o => o.Ignore())
                .ForMember(d => d.WorkScheduleTemplate, o => o.Ignore())
                .ForMember(d => d.Shift, o => o.Ignore());

            CreateMap<WorkScheduleTemplateAssignmentForStaffTeleRoom, WorkScheduleTemplateAssignmentForStaffTeleRoom>()
                .ForMember(d => d.WorkScheduleTemplateId, o => o.Ignore())
                .ForMember(d => d.WorkScheduleTemplate, o => o.Ignore())
                .ForMember(d => d.Shift, o => o.Ignore());
        }

        private void MapContract()
        {
            CreateMap<GetWorkScheduleDTO, GetWorkScheduleContract>();
            CreateMap<WorkScheduleAssignment, GetWorkScheduleAssignmentForSegmentDTO>()
                .ForMember(d => d.ShiftId, o => o.MapFrom(s => s.WorkSchedule!.ShiftId))
                .ForMember(d => d.WorkDate, o => o.MapFrom(s => s.WorkSchedule!.WorkDate))
                .ForMember(d => d.ShiftStartTime, o => o.MapFrom(s => s.WorkSchedule!.Shift!.StartTime))
                .ForMember(d => d.ShiftEndTime, o => o.MapFrom(s => s.WorkSchedule!.Shift!.EndTime))
                .ForMember(d => d.IsOvertimeShift, o => o.MapFrom(s => s.WorkSchedule!.Shift!.IsOvertime));
            CreateMap<GetWorkScheduleAssignmentDTO, GetWorkScheduleAssignmentContract>()
                .ForMember(d => d.LocationType, o => o.MapFrom(s => s.LocationType.ToString()));
            CreateMap<GetShiftDTO, GetShiftContract>();
            CreateMap<LeaveRequestApprovedEvent, LeaveRequestValidationDTO>()
                .ForMember(d => d.LeaveUnit, o => o.MapFrom(s => s.LeaveUnit ?? string.Empty));
            CreateMap<ValidateLeaveRequestEvent, LeaveRequestValidationDTO>();
            CreateMap<LeaveRequestApprovedEvent, LeaveRequestValidationDTO>();
            CreateMap<GetStaffDTO, GetStaffDataByUserIdContract>().ReverseMap();
            CreateMap<GetRoomDTO, GetRoomContract>().ReverseMap();
            CreateMap<GetTeleRoomDTO, GetTeleRoomContract>().ReverseMap();
            CreateMap<GetUserDataByListRoleContract, GetStaffDTO>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId));
            CreateMap<RoomDoctorInfoDTO, GetListTodayWorkingMedicalRoomContract.RoomDoctorInfo>();
        }

        private void MapHolidayRange()
        {
            CreateMap<CreateHolidayRangeDTO, UpsertHolidayDTO>()
                .ForMember(d => d.Day, o => o.Ignore())
                .ForMember(d => d.Month, o => o.Ignore());

            CreateMap<DateOnly, UpsertHolidayDTO>()
                .ForMember(d => d.Day, o => o.MapFrom(s => s.Day))
                .ForMember(d => d.Month, o => o.MapFrom(s => s.Month))
                .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
                .ForMember(d => d.Name, o => o.Ignore());
        }
    }
}
