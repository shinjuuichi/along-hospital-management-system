using InpatientResourceSvc.BLL.DTOs.FloorDTOs;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Interfaces
{
    public interface IFloorService
        : IBaseCrudService<UpsertFloorDTO, UpsertFloorDTO, GetFloorDTO>;
}