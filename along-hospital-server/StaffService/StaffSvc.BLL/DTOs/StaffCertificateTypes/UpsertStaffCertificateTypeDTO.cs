using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffCertificateTypes
{
    public class UpsertStaffCertificateTypeDTO : MapTo<StaffCertificateType>
    {
        public string? Name { get; set; }

        public string? ScopeOfPractice { get; set; }
    }
}