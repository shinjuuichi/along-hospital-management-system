using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.FloorDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;

namespace InpatientResourceSvc.BLL.Implements
{
    public class FloorService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Floor, UpsertFloorDTO, UpsertFloorDTO, GetFloorDTO>(
            unitOfWork,
            mapper,
            includes: ["Building"]),
          IFloorService
    {
        public override async Task<GetFloorDTO> CreateAsync(UpsertFloorDTO createDTO)
        {
            await this.CheckDuplicatedFloorNumberAsync(
                createDTO.BuildingId,
                createDTO.FloorNumber);

            return await base.CreateAsync(createDTO);
        }

        public override async Task<GetFloorDTO> UpdateAsync(int id, UpsertFloorDTO updateDTO)
        {
            await this.CheckDuplicatedFloorNumberAsync(
                updateDTO.BuildingId,
                updateDTO.FloorNumber,
                excludeFloorId: id);

            return await base.UpdateAsync(id, updateDTO);
        }

        private async Task CheckDuplicatedFloorNumberAsync(
            int buildingId,
            int floorNumber,
            int? excludeFloorId = null)
        {
            var exists = await _repository.AnyAsync(f =>
                (!excludeFloorId.HasValue || f.Id != excludeFloorId.Value)
                && f.BuildingId == buildingId
                && f.FloorNumber == floorNumber);

            if (exists)
            {
                throw new InvalidDataException($"Floor {floorNumber} already exists in BuildingId {buildingId}.");
            }
        }
    }
}