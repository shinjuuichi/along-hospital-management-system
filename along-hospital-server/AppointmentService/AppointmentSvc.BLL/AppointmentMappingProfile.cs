using AppointmentSvc.BLL.DTOs;
using AppointmentSvc.BLL.DTOs.GetAppointmentDTOs;
using AppointmentSvc.BLL.DTOs.StatisticsDTOs;
using AppointmentSvc.BLL.DTOs.TimeSlotDTOs;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.PaymentEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace AppointmentSvc.BLL
{
    public class AppointmentMappingProfile : BaseMappingProfile
    {
        public AppointmentMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<AppointmentStatisticsDTO, GetAppointmentStatisticsByDateRangeContract>();

            // DTO, Contract
            CreateMap<GetAppointmentDTO, GetAppointmentContract>()
                .ForMember(d => d.Time, opt => opt.MapFrom(s => s.TimeSlotSnapshot!.Time));
            CreateMap<GetTimeSlotDTO, GetTimeSlotContract>();
            CreateMap<GetSpecialtyByIdContract, GetAppointmentSpecialtyDTO>();
            CreateMap<GetPatientAllergyDataContractItem, GetAppointmentPatientAllergyDTO>();
            CreateMap<GetPatientDataByUserIdContract, GetAppointmentPatientDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));
            CreateMap<GetTeleSessionByAppointmentIdContract, GetAppointmentTeleSessionDTO>();

            // DTO, Event
            CreateMap<PaymentStatusChangedEvent, PaymentStatusChangedDTO>();
            CreateMap<GetAppointmentDTO, SendAppointmentReminderEmailEvent>()
                .ForMember(d => d.AppointmentId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Time, opt => opt.MapFrom(s => s.TimeSlotSnapshot!.Time))
                .ForMember(d => d.SpecialtyName, opt => opt.MapFrom(s => s.Specialty != null ? s.Specialty.Name : null))
                .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient != null ? s.Patient.Name : null));
        }
    }
}
