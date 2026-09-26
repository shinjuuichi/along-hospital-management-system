using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.BuildingDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Implements
{
    public class BuildingService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Building, UpsertBuildingDTO, UpsertBuildingDTO, GetBuildingDTO>(
            unitOfWork,
            mapper,
            includes: ["Floors"]),
                IBuildingService;
}