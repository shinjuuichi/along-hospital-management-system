using AutoMapper;
using FeedbackSvc.BLL.DTOs.StaffDTOs;
using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs
{
    public class GetFeedbackRespondDTO : MapFrom<FeedbackRespond>
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int FeedbackId { get; set; }

        public DateTime CreationDate { get; set; }

        public string? FeedbackStatus { get; set; }

        public int ResponderId { get; set; }

        public GetResponderDTO? GetResponderDTO { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<FeedbackRespond, GetFeedbackRespondDTO>()
                .ForMember(dest => dest.ResponderId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.FeedbackStatus, opt => opt.MapFrom(src => src.FeedbackStatus.ToString()));
        }
    }
}
