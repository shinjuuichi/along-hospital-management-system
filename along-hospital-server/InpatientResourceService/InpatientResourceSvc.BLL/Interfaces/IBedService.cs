using InpatientResourceSvc.BLL.DTOs.BedDTOs;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IBedService
        : IBaseCrudService<UpsertBedDTO, UpsertBedDTO, GetBedDTO>;
}