using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.QualificationDTOs;

namespace StaffSvc.BLL.Interfaces
{
    public interface IQualificationService : IBaseCrudService<CreateQualificationDTO, UpdateQualificationDTO, GetQualificationDTO>;
}