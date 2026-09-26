using AppointmentSvc.DAL.Models;
using AutoMapper;
using SharedLibrary.Base.Mappers;

namespace AppointmentSvc.BLL.DTOs.TimeSlotDTOs
{
    public class GetTimeSlotDTO : MapFrom<TimeSlot>
    {
        public int Id { get; set; }

        public TimeOnly Time { get; set; }

        public int CapacityPerDoctor { get; set; }

        public int AvailableCapacityForInPerson { get; set; }

        public int MaxCapacityForInPerson { get; set; }

        public int AvailableCapacityForTeleHealth { get; set; }

        public int MaxCapacityForTeleHealth { get; set; }

        public override void Mapping(Profile profile)
        {
            base.Mapping(profile);

            profile.CreateMap<GetTimeSlotDTO, TimeSlotSnapshot>();
        }
    }
}