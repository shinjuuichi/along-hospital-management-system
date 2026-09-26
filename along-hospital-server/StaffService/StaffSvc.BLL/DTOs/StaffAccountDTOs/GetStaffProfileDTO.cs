using AutoMapper;
using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffAccountDTOs
{
    public class GetStaffProfileDTO : MapFrom<Staff>
    {
        //Staff properties
        public int Id { get; set; }

        public string? SpecialtyName { get; set; }

        public string? QualificationName { get; set; }

        //User properties
        public string? Name { get; set; }

        public string? Image { get; set; }

        public string? Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Staff, GetStaffProfileDTO>()
                .ForMember(d => d.SpecialtyName, opt => opt.MapFrom(s => s.Specialty != null ? s.Specialty.Name : string.Empty))
                .ForMember(d => d.QualificationName, opt => opt.MapFrom(s => s.Qualification != null ? s.Qualification.Name : string.Empty));
        }
    }
}
