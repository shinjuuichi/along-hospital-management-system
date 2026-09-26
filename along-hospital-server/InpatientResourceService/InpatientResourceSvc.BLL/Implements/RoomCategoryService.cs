using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.RoomCategoryDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.BLL.Implements
{
    public class RoomCategoryService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<RoomCategory, UpsertRoomCategoryDTO, UpsertRoomCategoryDTO, GetRoomCategoryDTO>(
            unitOfWork, mapper, includes: [$"{nameof(RoomCategory.RoomCategoryRoleMappings)}.{nameof(RoomCategoryRoleMapping.RoomCategoryRole)}"]),
            IRoomCategoryService
    {
        private readonly IGenericRepository<RoomCategoryRole> _roomCategoryRoleRepository
            = unitOfWork.Repository<RoomCategoryRole>();

        public override async Task<GetRoomCategoryDTO> CreateAsync(UpsertRoomCategoryDTO createDTO)
        {
            var entity = _mapper.Map<RoomCategory>(createDTO);
            entity.RoomCategoryRoleMappings = await this.BuildRoleMappingsAsync(createDTO.Roles);
            var resultEntity = await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return await base.GetByIdAsync(resultEntity.Id);
        }

        public override async Task<GetRoomCategoryDTO> UpdateAsync(int id, UpsertRoomCategoryDTO updateDTO)
        {
            var entity = await _repository.GetByIdAsync(id,
                    [$"{nameof(RoomCategory.RoomCategoryRoleMappings)}"])
                ?? throw new DataNotFoundException(typeof(RoomCategory), id);

            _mapper.Map(updateDTO, entity);

            entity.RoomCategoryRoleMappings.Clear();
            var newMappings = await this.BuildRoleMappingsAsync(updateDTO.Roles);
            foreach (var mapping in newMappings)
            {
                entity.RoomCategoryRoleMappings.Add(mapping);
            }

            _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();

            return await base.GetByIdAsync(id);
        }

        #region Helper Methods
        private async Task<ICollection<RoomCategoryRoleMapping>> BuildRoleMappingsAsync(List<string> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                throw new ValidationFailureException("At least one role is required.");
            }

            var roleEnums = roles
                .Select(r => Enum.Parse<RoleEnum>(r))
                .Distinct()
                .ToList();

            var existingRoles = await _roomCategoryRoleRepository.GetAllAsync(
                r => roleEnums.Contains(r.Role));

            var existingRoleEnums = existingRoles.Select(r => r.Role).ToHashSet();

            var newRoles = roleEnums
                .Where(r => !existingRoleEnums.Contains(r))
                .Select(r => new RoomCategoryRole { Role = r })
                .ToList();

            if (newRoles.Count > 0)
            {
                await _roomCategoryRoleRepository.AddRangeAsync(newRoles);
                await _unitOfWork.SaveChangeAsync();
            }

            return existingRoles.Concat(newRoles)
                .Select(r => new RoomCategoryRoleMapping { RoomCategoryRoleId = r.Id })
                .ToList();
        }
        #endregion
    }
}
