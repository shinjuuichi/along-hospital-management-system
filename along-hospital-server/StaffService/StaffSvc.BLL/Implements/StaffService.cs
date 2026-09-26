using AutoMapper;
using Gridify.Builder;
using MessageBroker.Contracts.AuthAccountContracts;
using MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.AuthAccountContracts.UpdateAccountContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using MessageBroker.Events.StaffEvents;
using MessageBroker.Events.UserEvents;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using StaffSvc.BLL.DTOs.StaffAccountDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.BLL.StateMachines;
using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;
using System.Linq.Expressions;

namespace StaffSvc.BLL.Implements
{
    public class StaffService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadFileService uploadFileService,
        IMessageBus messageBus)
        : BaseService<Staff, CreateStaffAndAccountDTO, UpdateStaffAndAccountDTO, GetStaffAndAccountDTO>(
            unitOfWork,
            mapper,
            uploadFileService,
            [nameof(Specialty), nameof(Qualification)]), IStaffService
    {
        private readonly IMessageBus _messageBus = messageBus;

        #region Primary Methods
        public async Task CreateStaffAndAccountAsync(CreateStaffAndAccountDTO createStaffAndAccountDTO)
        {
            EnumUtil.ParseEnum<BankCodeEnum>(createStaffAndAccountDTO.BankCode);

            if (createStaffAndAccountDTO.Image == null)
            {
                throw new InvalidDataException("Image cannot be null.");
            }

            var staffImage = await this.UploadStaffImageAsync(createStaffAndAccountDTO.Image);

            try
            {
                var createUserToAuthEvent = _mapper.Map<CreateUserToAuthEvent>(createStaffAndAccountDTO) with { Image = staffImage };
                var createUserToAuthContract = await _messageBus.RequestAsync<CreateUserToAuthEvent, CreateUserToAuthContract>(createUserToAuthEvent);
                createStaffAndAccountDTO.Id = createUserToAuthContract.Id;

                await _unitOfWork.BeginTransactionAsync();

                await this.CreateAsync(createStaffAndAccountDTO);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                await _uploadFileService!.DeleteAsync(staffImage);

                throw;
            }
        }

        public async Task UpdateStaffAndAccountAsync(int staffId, UpdateStaffAndAccountDTO updateStaffAndAccountDTO)
        {
            EnumUtil.ParseEnum<BankCodeEnum>(updateStaffAndAccountDTO.BankCode);

            string? staffImage;
            var isNewImageProvided = updateStaffAndAccountDTO.Image != null;
            if (isNewImageProvided)
            {
                staffImage = await this.UploadStaffImageAsync(updateStaffAndAccountDTO.Image!);
            }
            else
            {
                var staff = await base.GetByIdAsync(staffId);
                staffImage = staff.Image;
            }

            try
            {
                var updateUserToAuthEvent = _mapper.Map<UpdateUserToAuthEvent>(updateStaffAndAccountDTO) with { Image = staffImage, UserId = staffId };
                await _messageBus.RequestAsync<UpdateUserToAuthEvent, UpdateUserToAuthContract>(updateUserToAuthEvent);

                await _unitOfWork.BeginTransactionAsync();

                await this.UpdateAsync(staffId, updateStaffAndAccountDTO);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                if (isNewImageProvided)
                {
                    await _uploadFileService!.DeleteAsync(staffImage);
                }

                throw;
            }
        }

        public async Task<PaginationResult<GetStaffProfileDTO>> GetStaffsByRoleAsync(string role, FilterDTO filterDTO)
        {
            var getListUserIdByNameContainsAndRoleContract = await _messageBus
                .RequestAsync<GetListUserIdByNameContainsAndRoleEvent, GetListUserIdByNameContainsAndRoleContract>(new GetListUserIdByNameContainsAndRoleEvent
                {
                    Role = role
                });

            var userIds = getListUserIdByNameContainsAndRoleContract.Ids.ToList();
            if (userIds.Count == 0)
            {
                return new PaginationResult<GetStaffProfileDTO>(0, filterDTO.PageSize, []);
            }

            var (total, staffsByRole) = await _repository.GetAllPaginatedAsync(
                s => userIds.Contains(s.Id),
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var staffs = _mapper.Map<List<GetStaffProfileDTO>>(staffsByRole);
            var staffIds = staffs.Select(s => s.Id).ToList();
            if (staffIds.Count == 0)
            {
                return new PaginationResult<GetStaffProfileDTO>(total, filterDTO.PageSize, staffs);
            }

            var userContracts = await _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new GetListUserDataByUserIdsEvent
            {
                UserIds = staffIds
            });

            var userDict = userContracts.Data.ToDictionary(u => u.UserId);

            foreach (var staff in staffs)
            {
                if (userDict.TryGetValue(staff.Id, out var userData))
                {
                    _mapper.Map(userData, staff);
                }
            }

            return new PaginationResult<GetStaffProfileDTO>(total, filterDTO.PageSize, staffs);
        }

        public async Task UpdateStatusAsync(List<int> staffIds, StaffStatusEnum newStatus)
        {
            if (staffIds.Count == 0)
            {
                throw new InvalidDataException("StaffIds cannot be empty.");
            }

            var staffs = await _repository.GetAllAsync(s => staffIds.Contains(s.Id));

            var terminatedStaffIds = new List<int>();

            foreach (var staff in staffs)
            {
                var stateMachine = new StaffStatusStateMachine(staff);
                if (!stateMachine.CanFire(newStatus))
                {
                    throw new InvalidDataException($"Cannot change staff status from '{staff.Status}' to '{newStatus}'");
                }

                stateMachine.Fire(newStatus);

                if (newStatus == StaffStatusEnum.Terminated)
                {
                    terminatedStaffIds.Add(staff.Id);
                }
            }

            _repository.UpdateRange(staffs);
            await _unitOfWork.SaveChangeAsync();

            if (newStatus == StaffStatusEnum.Terminated)
            {
                if (terminatedStaffIds.Count > 0)
                {
                    await _messageBus.PublishAsync(new TerminateStaffEvent
                    {
                        StaffIds = terminatedStaffIds
                    });
                }
            }
        }
        #endregion

        #region Helper Methods
        private async Task<string> UploadStaffImageAsync(IFormFile image)
        {
            return await _uploadFileService!.UploadAsync(image, nameof(Staff));
        }

        private async Task<Expression<Func<Staff, bool>>> ApplyStaffFilterAsync(Expression<Func<Staff, bool>> filterExpr, StaffFilterDTO staffFilterDTO)
        {
            var hasAuthFilter = !string.IsNullOrWhiteSpace(staffFilterDTO.Email) || !string.IsNullOrWhiteSpace(staffFilterDTO.Phone);
            var hasUserFilter = !string.IsNullOrWhiteSpace(staffFilterDTO.Name) || staffFilterDTO.Role != null || staffFilterDTO.Gender != null;

            List<int> userIdsFromAuth = [];
            List<int> userIdsFromUser = [];

            if (hasAuthFilter)
            {
                var authEvent = _mapper.Map<StaffFilterDTO, GetListUserIdByFilterAuthAccountEntityEvent>(staffFilterDTO);
                var authResult = await _messageBus.RequestAsync<GetListUserIdByFilterAuthAccountEntityEvent, GetListUserIdByFilterAuthAccountEntityContract>(authEvent);

                userIdsFromAuth = authResult.UserIds.Where(id => id.HasValue).Select(id => id!.Value).ToList();
                if (userIdsFromAuth.Count == 0)
                {
                    return s => false;
                }
            }

            if (hasUserFilter)
            {
                var userEvent = _mapper.Map<StaffFilterDTO, GetListUserIdByFilterUserEntityEvent>(staffFilterDTO);
                var userResult = await _messageBus.RequestAsync<GetListUserIdByFilterUserEntityEvent, GetListUserIdByFilterUserEntityContract>(userEvent);

                userIdsFromUser = userResult.UserIds.ToList();
                if (userIdsFromUser.Count == 0)
                {
                    return s => false;
                }
            }

            if (!hasAuthFilter && !hasUserFilter)
            {
                return filterExpr;
            }

            var combinedUserIds =
                hasAuthFilter && hasUserFilter
                    ? userIdsFromAuth.Intersect(userIdsFromUser).ToList()
                    : hasAuthFilter
                        ? userIdsFromAuth
                        : userIdsFromUser;

            return combinedUserIds.Count == 0 ? (s => false) : filterExpr.And(s => combinedUserIds.Contains(s.Id));
        }

        private async Task<List<GetStaffAndAccountDTO>> CombinedUserAndStaffDataAsync(List<GetStaffAndAccountDTO> staffs)
        {
            var staffIds = staffs.Select(s => s.Id).ToList();
            if (staffIds.Count == 0)
            {
                return staffs;
            }

            var userContracts = await _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new GetListUserDataByUserIdsEvent
            {
                UserIds = staffIds
            });

            var userDict = userContracts.Data.ToDictionary(u => u.UserId);

            foreach (var staff in staffs)
            {
                if (userDict.TryGetValue(staff.Id, out var userData))
                {
                    _mapper.Map(userData, staff);
                }
            }

            return staffs;
        }
        #endregion

        #region Override Methods
        public override async Task<GetStaffAndAccountDTO> GetByIdAsync(int id)
        {
            var staffAndAccountDTO = await base.GetByIdAsync(id);
            var userAndAuthDataContract = await _messageBus.RequestAsync<GetUserDataByUserIdEvent, GetUserDataByUserIdContract>(new GetUserDataByUserIdEvent
            {
                UserId = id
            });

            _mapper.Map(userAndAuthDataContract, staffAndAccountDTO);
            return staffAndAccountDTO;
        }

        public override async Task<List<GetStaffAndAccountDTO>> GetAllAsync()
        {
            var staffs = await _repository.GetAllAsync(s => s.Status == StaffStatusEnum.Active, _includes);
            var staffDtos = _mapper.Map<List<GetStaffAndAccountDTO>>(staffs);
            return await this.CombinedUserAndStaffDataAsync(staffDtos);
        }

        public override async Task<List<GetStaffAndAccountDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var staffs = await base.GetAllByIdsAsync(ids);
            return await this.CombinedUserAndStaffDataAsync(staffs);
        }

        public override async Task<PaginationResult<GetStaffAndAccountDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            Expression<Func<Staff, bool>> filterExpr = s => true;
            filterExpr = await this.ApplyStaffFilterAsync(filterExpr, (StaffFilterDTO)filterDTO);

            var (total, staffs) = await _repository.GetAllPaginatedAsync(
                filterExpr,
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var staffDtos = _mapper.Map<List<GetStaffAndAccountDTO>>(staffs);
            var combined = await this.CombinedUserAndStaffDataAsync(staffDtos);

            return new PaginationResult<GetStaffAndAccountDTO>(total, filterDTO.PageSize, combined);
        }

        public override async Task DeleteAsync(int staffId)
        {
            await base.DeleteAsync(staffId);
            await _messageBus.PublishAsync(new DeleteUserByIdWhenCrashingEvent { Id = staffId });
        }
        #endregion

        #region Checks Exist
        public async Task<bool> CheckExistByIdAsync(int id)
        {
            return await _repository.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> CheckExistByIdsAsync(List<int> ids)
        {
            var existingCount = await _repository.CountAsync(s => ids.Contains(s.Id));
            return existingCount == ids.Count;
        }

        public async Task<List<int>> GetActiveStaffIdsByIdsAsync(List<int> ids)
        {
            if (ids.Count == 0)
            {
                return [];
            }

            var staffIds = await _repository.GetAllAsync(s => ids.Contains(s.Id) && s.Status == StaffStatusEnum.Active);
            return staffIds.Select(s => s.Id).ToList();
        }
        #endregion
    }
}
