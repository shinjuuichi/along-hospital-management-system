using InpatientResourceSvc.BLL.DTOs.BuildingDTOs;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IBuildingService
        : IBaseCrudService<UpsertBuildingDTO, UpsertBuildingDTO, GetBuildingDTO>;
}