using AutoMapper;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.PayrollPolicyDTOs
{
    public class UpdatePayrollPolicyDTO : MapTo<PayrollPolicy>
    {
        #region Primary properties
        public string? Name { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string? PayrollPolicyStatus { get; set; }
        #endregion

        #region Foreign keys
        public List<int> StaffIds { get; set; } = [];

        public int? AllowanceTypeId { get; set; }

        public int? DeductionTypeId { get; set; }
        #endregion

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePayrollPolicyDTO, PayrollPolicy>()
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping)
                .ForMember(dest => dest.PayrollPolicyStaffs,
                    opt => opt.MapFrom(src => src.StaffIds.Select(staffId => new PayrollPolicyStaff
                    {
                        StaffId = staffId
                    }).ToList()));
        }
    }
}