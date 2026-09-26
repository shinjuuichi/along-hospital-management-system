using AutoMapper;
using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffContractDTOs
{
    public class GetStaffContractDTO : MapFrom<StaffContract>
    {
        public int Id { get; set; }

        public string? ContractCode { get; set; }

        public string? ContractType { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public double HourlyRate { get; set; }

        public int WorkingHoursPerWeek { get; set; }

        public string? Status { get; set; }

        public DateOnly? SignedDate { get; set; }

        public string? SignatureImage { get; set; }

        public double InsuranceSalaryRate { get; set; }

        public int StaffId { get; set; }

        public string? StaffName { get; set; }

        public string? StaffImage { get; set; }

        public int RegionalWageId { get; set; }

        public int RegionalWageCode { get; set; }

        public double RegionalWageMonthlyWage { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<StaffContract, GetStaffContractDTO>()
                .ForMember(dest => dest.RegionalWageCode,
                    opt => opt.MapFrom(src => src.RegionalWage != null ? src.RegionalWage.Code : 0))
                .ForMember(dest => dest.RegionalWageMonthlyWage,
                    opt => opt.MapFrom(src => src.RegionalWage != null ? src.RegionalWage.MonthlyWage : 0));
        }
    }
}
