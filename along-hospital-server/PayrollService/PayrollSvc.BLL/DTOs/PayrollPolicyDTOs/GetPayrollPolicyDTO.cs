using AutoMapper;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace PayrollSvc.BLL.DTOs.PayrollPolicyDTOs
{
    public class GetPayrollPolicyDTO : MapFrom<PayrollPolicy>
    {
        #region Primary properties
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string? PayrollPolicyStatus { get; set; }
        #endregion

        #region Foreign keys
        public List<int> StaffIds { get; set; } = [];

        public int? AllowanceTypeId { get; set; }
        public string? AllowanceTypeName { get; set; }

        public int? DeductionTypeId { get; set; }
        public string? DeductionTypeName { get; set; }
        #endregion

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<PayrollPolicy, GetPayrollPolicyDTO>()
                .ForMember(d => d.StaffIds, o => o.MapFrom(s =>
                    s.PayrollPolicyStaffs.Select(x => x.StaffId).ToList()));
        }
    }
}
