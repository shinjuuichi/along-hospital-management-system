using MessageBroker.Contracts.RecruitmentContracts;
using MessageBroker.Events.SendEmailEvents;
using RecruitmentSvc.BLL.DTOs.InterviewDTOs;
using RecruitmentSvc.BLL.DTOs.StatisticsDTOs;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace RecruitmentSvc.BLL
{
    public class RecruitmentMappingProfile : BaseMappingProfile
    {
        public RecruitmentMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<RecruitmentStatisticsDTO, GetRecruitmentStatisticsByDateRangeContract>();

            CreateMap<JobApplication, GetInterviewMailDTO>()
                .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPosting!.Title));

            CreateMap<Interview, GetInterviewMailDTO>()
                .ForMember(dest => dest.InterviewTypeName, opt => opt.MapFrom(src => src.InterviewType!.Name))
                .ForMember(dest => dest.InterviewTypeDescription, opt => opt.MapFrom(src => src.InterviewType!.Description));

            CreateMap<GetInterviewDTO, GetInterviewMailDTO>(); 
            CreateMap<GetInterviewMailDTO, SendInterviewEmailEvent>();
            CreateMap<GetInterviewMailDTO, SendInterviewResultEmailEvent>();
        }
    }
}
