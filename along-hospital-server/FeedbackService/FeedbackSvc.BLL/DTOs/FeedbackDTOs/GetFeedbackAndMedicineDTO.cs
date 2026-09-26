using AutoMapper;
using FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs;
using FeedbackSvc.BLL.DTOs.MedicineDTOs;
using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackDTOs
{
    public class GetFeedbackAndMedicineDTO : MapFrom<Feedback>
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public double Rating { get; set; }

        public int PatientId { get; set; }

        public string? PatientName { get; set; }

        public string? FeedbackReplyStatus { get; set; }

        public List<GetFeedbackRespondDTO> FeedbackResponds { get; set; } = [];

        public GetMedicineDTO? GetMedicineDTO { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Feedback, GetFeedbackAndMedicineDTO>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForPath(dest => dest.GetMedicineDTO!.MedicineId, opt => opt.MapFrom(src => src.MedicineId));
        }
    }
}