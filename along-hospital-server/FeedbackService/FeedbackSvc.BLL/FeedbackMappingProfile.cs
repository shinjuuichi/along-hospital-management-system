using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.FeedbackEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace FeedbackSvc.BLL
{
    public class FeedbackMappingProfile : BaseMappingProfile
    {
        public FeedbackMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MappingFeedback();
            MappingFeedbackRespond();
            MappingFeedbackManagement();
        }

        private void MappingFeedback()
        {
            //DTO to Contract
            CreateMap<GetUserDataByUserIdContract, GetFeedbackDTO>()
                .ForPath(dest => dest.GetUserDTO!.PatientName, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.GetUserDTO!.PatientImage, opt => opt.MapFrom(src => src.Image))
                .ForPath(dest => dest.GetUserDTO!.PatientRole, opt => opt.MapFrom(src => src.Role));

            //DTO to Event
            CreateMap<GetFeedbackDTO, PredictFeedbackTypeEvent>();
            CreateMap<GetFeedbackDTO, PredictFeedbackToxicEvent>();
        }

        private void MappingFeedbackRespond()
        {
            //Contract to DTO
            CreateMap<GetUserDataByUserIdContract, GetFeedbackRespondDTO>()
                .ForPath(dest => dest.GetResponderDTO!.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.GetResponderDTO!.Image, opt => opt.MapFrom(src => src.Image))
                .ForPath(dest => dest.GetResponderDTO!.Role, opt => opt.MapFrom(src => src.Role))
                .ForPath(dest => dest.GetResponderDTO!.Email, opt => opt.MapFrom(src => src.Email));

            //DTO to Event
            CreateMap<GetFeedbackRespondDTO, SendFeedbackRespondEvent>()
                .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.GetResponderDTO!.Name));
            CreateMap<GetFeedbackRespondDTO, PredictFeedbackRespondToxicEvent>();
        }

        private void MappingFeedbackManagement()
        {
            //Contract to DTO
            CreateMap<GetMedicineByIdContract, GetFeedbackAndMedicineDTO>()
                .ForPath(dest => dest.GetMedicineDTO!.MedicineName, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.GetMedicineDTO!.MedicineBrand, opt => opt.MapFrom(src => src.Brand))
                .ForPath(dest => dest.GetMedicineDTO!.MedicineId, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.GetMedicineDTO!.MedicineImages, opt => opt.MapFrom(src => src.MedicineImages))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<GetUserDataByUserIdContract, GetFeedbackAndMedicineDTO>()
                .ForPath(dest => dest.PatientName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
