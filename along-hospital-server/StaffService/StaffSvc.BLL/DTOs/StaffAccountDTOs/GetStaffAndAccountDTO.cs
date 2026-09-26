using AutoMapper;
using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffAccountDTOs
{
    public class GetStaffAndAccountDTO : MapFrom<Staff>
    {
        //Staff properties
        public int Id { get; set; }

        public string? SpecialtyName { get; set; }

        public int SpecialtyId { get; set; }

        public string? QualificationName { get; set; }

        public int QualificationId { get; set; }

        public string? BankCode { get; set; }

        public string? AccountNumber { get; set; }

        public int DependentQuantity { get; set; }

        public string? Status { get; set; }

        //User properties
        public string? Role { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public DateOnly DateOfBirth { get; set; }

        //Auth properties
        public string? Phone { get; set; }

        public string? Email { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Staff, GetStaffAndAccountDTO>()
                .ForMember(d => d.SpecialtyName, opt => opt.MapFrom(s => s.Specialty != null ? s.Specialty.Name : string.Empty))
                .ForMember(d => d.QualificationName, opt => opt.MapFrom(s => s.Qualification != null ? s.Qualification.Name : string.Empty));
        }
    }
}
