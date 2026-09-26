using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.StaffCertificateTypes;

namespace StaffSvc.BLL.Interfaces
{
    public interface IStaffCertificateTypeService : IBaseCrudService<UpsertStaffCertificateTypeDTO, UpsertStaffCertificateTypeDTO, GetStaffCertificateTypeDTO>;
}