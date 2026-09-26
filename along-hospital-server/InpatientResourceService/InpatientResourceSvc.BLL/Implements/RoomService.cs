using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.RoomDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using System.Linq.Expressions;

namespace InpatientResourceSvc.BLL.Implements
{
    public class RoomService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : BaseService<Room, CreateRoomDTO, UpdateRoomDTO, GetRoomDTO>(unitOfWork, mapper,
                    includes: [nameof(Room.Floor),
                    nameof(Room.RoomCategory), nameof(Room.Beds),
                    $"{nameof(Room.Floor)}.{nameof(Floor.Building)}",
                    $"{nameof(Room.RoomCategory)}.{nameof(RoomCategory.RoomCategoryRoleMappings)}.{nameof(RoomCategoryRoleMapping.RoomCategoryRole)}"]),
            IRoomService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<Floor> _floorRepository = unitOfWork.Repository<Floor>();

        public override async Task<GetRoomDTO> CreateAsync(CreateRoomDTO createDTO)
        {
            await this.ValidateSpecialtyAsync(createDTO.SpecialtyId);

            var floor = await _floorRepository.GetByIdAsync(createDTO.FloorId, includes: [nameof(Floor.Building)])
                ?? throw new DataNotFoundException(typeof(Floor), createDTO.FloorId);

            createDTO.Code = await this.GenerateRoomCodeAsync(floor, createDTO.FloorId);

            return await base.CreateAsync(createDTO);
        }

        public override async Task<GetRoomDTO> UpdateAsync(int id, UpdateRoomDTO updateDTO)
        {
            await this.ValidateSpecialtyAsync(updateDTO.SpecialtyId);

            return await base.UpdateAsync(id, updateDTO);
        }

        public override async Task<GetRoomDTO> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            await this.RequestValueForRoomDTOsAsync([result]);

            return result;
        }

        public override async Task<List<GetRoomDTO>> GetAllAsync()
        {
            var rooms = await _repository.GetAllAsync(
                  r => r.Status == RoomStatusEnum.Active,
                  _includes);
            var results = _mapper.Map<List<GetRoomDTO>>(rooms);
            await this.RequestValueForRoomDTOsAsync(results);

            return results;
        }

        public override async Task<List<GetRoomDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var roomDTOs = await base.GetAllByIdsAsync(ids);
            await this.RequestValueForRoomDTOsAsync(roomDTOs);

            return roomDTOs;
        }

        public async Task<List<GetRoomDTO>> GetAllByRolesAsync()
        {
            var rooms = await _repository.GetAllAsync(this.GetAllFilter(), _includes);
            var results = _mapper.Map<List<GetRoomDTO>>(rooms);
            await this.RequestValueForRoomDTOsAsync(results);

            return results;
        }

        public override async Task<PaginationResult<GetRoomDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var results = await base.GetAllPaginatedAsync(filterDTO);
            await this.RequestValueForRoomDTOsAsync(results.Collection);

            return results;
        }

        #region Helper methods
        private async Task ValidateSpecialtyAsync(int specialtyId)
        {
            await _messageBus.RequestAsync
                <CheckSpecialtyExistByIdEvent, CheckSpecialtyExistByIdContract>
                    (new CheckSpecialtyExistByIdEvent { SpecialtyId = specialtyId });
        }

        private async Task RequestValueForRoomDTOsAsync(List<GetRoomDTO> roomDTOs)
        {
            var specialtyIds = roomDTOs.Select(r => r.SpecialtyId).Distinct().ToList();

            if (specialtyIds.Count == 0)
            {
                return;
            }

            var specialtyContract = await _messageBus.RequestAsync
                <GetListSpecialtyDataByIdsEvent, GetListSpecialtyDataByIdsContract>
                    (new GetListSpecialtyDataByIdsEvent { SpecialtyIds = specialtyIds });

            var specialtyDict = specialtyContract.Data.ToDictionary(s => s.Id);

            foreach (var room in roomDTOs.Where(r => specialtyDict.ContainsKey(r.SpecialtyId)))
            {
                var specialty = specialtyDict[room.SpecialtyId];
                _mapper.Map(specialty, room);
            }
        }

        private Expression<Func<Room, bool>> GetAllFilter()
        {
            if (_currentUserService.Role == RoleEnum.HR)
            {
                return room => room.Status == RoomStatusEnum.Active
                    && room.RoomCategory != null
                    && room.RoomCategory.RoomCategoryRoleMappings.Any(mapping =>
                        mapping.RoomCategoryRole != null && mapping.RoomCategoryRole.Role != RoleEnum.Patient);
            }

            return room => room.Status == RoomStatusEnum.Active
                && room.RoomCategory != null
                && room.RoomCategory.RoomCategoryRoleMappings.Any(mapping =>
                    mapping.RoomCategoryRole != null && mapping.RoomCategoryRole.Role == RoleEnum.Patient);
        }

        // Generate code based on building's first letter, floor number, and the next available room number => B203
        private async Task<string> GenerateRoomCodeAsync(Floor floor, int floorId)
        {
            var existingRooms = await _repository.GetAllAsync(r => r.FloorId == floorId);
            int maxRoomNumber = existingRooms
                .Select(r => int.TryParse(r.Code[^2..], out int num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();
            int nextRoomNumber = maxRoomNumber + 1;

            string buildingLetter = floor?.Building?.Name[..1].ToUpper() ?? string.Empty;
            string generatedCode;

            do
            {
                generatedCode = $"{buildingLetter}{floor?.FloorNumber}{nextRoomNumber:D2}";
                var existingRoom = await _repository.GetByConditionAsync(r => r.Code == generatedCode);
                if (existingRoom == null)
                {
                    break;
                }

                nextRoomNumber++;
            } while (true);

            return generatedCode;
        }
        #endregion
    }
}
