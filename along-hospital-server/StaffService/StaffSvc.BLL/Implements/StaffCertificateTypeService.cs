using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.StaffCertificateTypes;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class StaffCertificateTypeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
            : BaseService<StaffCertificateType, UpsertStaffCertificateTypeDTO, UpsertStaffCertificateTypeDTO, GetStaffCertificateTypeDTO>(
                unitOfWork,
                mapper),
                IStaffCertificateTypeService;
}