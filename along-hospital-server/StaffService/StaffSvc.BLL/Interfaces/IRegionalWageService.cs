using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.RegionalWageDTOs;

namespace StaffSvc.BLL.Interfaces
{
    public interface IRegionalWageService : IBaseCrudService<CreateRegionalWageDTO, UpdateRegionalWageDTO, GetRegionalWageDTO>;
}