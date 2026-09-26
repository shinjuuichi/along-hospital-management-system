using AutoMapper;
using FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs;
using FeedbackSvc.BLL.DTOs.PatientDTOs;
using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackDTOs
{
    public class GetFeedbackDTO : MapFrom<Feedback>
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public double Rating { get; set; }

        public int MedicineId { get; set; }

        public int PatientId { get; set; }

        public DateTime CreationDate { get; set; }

        public string? FeedbackStatus { get; set; }

        public List<GetFeedbackRespondDTO> FeedbackResponds { get; set; } = [];

        public GetUserDTO? GetUserDTO { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Feedback, GetFeedbackDTO>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}