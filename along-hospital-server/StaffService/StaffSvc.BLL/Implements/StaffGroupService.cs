using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using StaffSvc.BLL.DTOs.StaffGroupDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class StaffGroupService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStaffService staffService)
            : BaseService<
                StaffGroup,
                UpsertStaffGroupDTO,
                UpsertStaffGroupDTO,
                GetStaffGroupDTO>(
                    unitOfWork,
                    mapper,
                    includes: [nameof(StaffGroup.StaffGroupMembers)]),
                    IStaffGroupService
    {
        private readonly IStaffService _staffService = staffService;

        private readonly IGenericRepository<StaffGroupMember> _staffGroupMemberRepository = unitOfWork.Repository<StaffGroupMember>();

        #region Get Operations
        public override async Task<List<GetStaffGroupDTO>> GetAllAsync()
        {
            var staffGroupDTOs = await base.GetAllAsync();
            await this.RequestStaffMemberValueForStaffGroups(staffGroupDTOs);
            return staffGroupDTOs;
        }

        public override async Task<PaginationResult<GetStaffGroupDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var staffGroupDTOs = await base.GetAllPaginatedAsync(filterDTO);
            await this.RequestStaffMemberValueForStaffGroups(staffGroupDTOs.Collection);
            return staffGroupDTOs;
        }

        public override async Task<List<GetStaffGroupDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var staffGroupDTOs = await base.GetAllByIdsAsync(ids);
            await this.RequestStaffMemberValueForStaffGroups(staffGroupDTOs);
            return staffGroupDTOs;
        }

        public override async Task<GetStaffGroupDTO> GetByIdAsync(int id)
        {
            var staffGroupDTO = await base.GetByIdAsync(id);
            await this.RequestStaffMemberValueForStaffGroups([staffGroupDTO]);
            return staffGroupDTO;
        }
        #endregion

        #region Action Operations
        public override async Task<GetStaffGroupDTO> CreateAsync(UpsertStaffGroupDTO createDTO)
        {
            createDTO.StaffIds = createDTO.StaffIds.Distinct().ToList();
            await this.ValidateStaffGroupMembers(createDTO.StaffIds);

            return await base.CreateAsync(createDTO);
        }

        public override async Task<GetStaffGroupDTO> UpdateAsync(int id, UpsertStaffGroupDTO updateDTO)
        {
            updateDTO.StaffIds = updateDTO.StaffIds.Distinct().ToList();
            await this.ValidateStaffGroupMembers(updateDTO.StaffIds);

            var entity = await _repository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(StaffGroup), id);

            _staffGroupMemberRepository.RemoveRange(entity.StaffGroupMembers.ToList());

            _mapper.Map(updateDTO, entity);

            var resultEntity = _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(resultEntity.Id);
        }
        #endregion

        public async Task<bool> CheckExistByIdAsync(int id)
        {
            return await _repository.AnyAsync(sg => sg.Id == id);
        }

        #region Private Methods
        private async Task ValidateStaffGroupMembers(List<int> staffIds)
        {
            if (staffIds.Count == 0)
            {
                throw new InvalidDataException("Staff group must have at least one staff member.");
            }

            var isAllStaffExist = await _staffService.CheckExistByIdsAsync(staffIds);
            if (!isAllStaffExist)
            {
                throw new DataNotFoundException("One or more staff members do not exist.");
            }
        }

        private async Task RequestStaffMemberValueForStaffGroups(List<GetStaffGroupDTO> staffGroupDTOs)
        {
            var distinctStaffIds = staffGroupDTOs
                .SelectMany(sg => sg.StaffGroupMembers.Select(sgm => sgm.StaffId))
                .Distinct()
                .ToList();

            if (distinctStaffIds.Count == 0)
            {
                return;
            }

            var staffDTOs = await _staffService.GetAllByIdsAsync(distinctStaffIds);
            var staffDTOsDict = staffDTOs.ToDictionary(s => s.Id);

            foreach (var staffGroupDTO in staffGroupDTOs)
            {
                foreach (var staffGroupMemberDTO in staffGroupDTO.StaffGroupMembers)
                {
                    if (staffDTOsDict.TryGetValue(staffGroupMemberDTO.StaffId, out var staffDTO))
                    {
                        staffGroupMemberDTO.Staff = staffDTO;
                    }
                }
            }
        }
        #endregion
    }
}