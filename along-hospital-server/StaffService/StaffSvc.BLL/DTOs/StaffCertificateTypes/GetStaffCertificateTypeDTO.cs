using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffCertificateTypes
{
    public class GetStaffCertificateTypeDTO : MapFrom<StaffCertificateType>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ScopeOfPractice { get; set; }
    }
}