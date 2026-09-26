using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.PatientEvents;
using MessageBroker.Events.StaffEvents;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.Interfaces;
using UserSvc.DAL.Enums;
using UserSvc.DAL.Models;

namespace UserSvc.BLL.Implements
{
    public class UserService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IUploadFileService _uploadFileService,
        ICurrentUserService _currentUserService,
        IMessageBus _messageBus)
        : BaseService<User, CreateUserDTO, UpdateUserDTO, GetUserDTO>(_unitOfWork, _mapper, _uploadFileService), IUserService
    {
        private IGenericRepository<User> _userRepository => _unitOfWork.Repository<User>();

        public async Task<UserProfileDTO> GetUserProfileAsync()
        {
            var userId = _currentUserService.UserId;
            var role = _currentUserService.Role;

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new DataNotFoundException(typeof(User), userId);

            var authDataContract = await _messageBus.RequestAsync<
                GetAuthDataByUserIdEvent,
                GetAuthDataByUserIdContract>(new() { UserId = userId });

            var baseDto = _mapper.Map<UserProfileDTO>(user);
            _mapper.Map(authDataContract, baseDto);

            switch (role)
            {
                case RoleEnum.Patient:
                    {
                        var contract = await _messageBus.RequestAsync<GetPatientProfileEvent, GetPatientProfileContract>(
                            new GetPatientProfileEvent { PatientId = userId }
                        );
                        var dto = _mapper.Map<PatientProfileDTO>(baseDto);
                        _mapper.Map(contract, dto);
                        return dto;
                    }
                case RoleEnum.Doctor:
                    {
                        var contract = await _messageBus.RequestAsync<GetStaffProfileEvent, GetStaffProfileContract>(
                            new GetStaffProfileEvent { StaffId = userId }
                        );
                        var dto = _mapper.Map<DoctorProfileDTO>(baseDto);
                        _mapper.Map(contract, dto);
                        return dto;
                    }
                default:
                    return baseDto;
            }
        }

        public async Task UpdateProfileAsync(UpdateProfileUserDTO dto)
        {
            var userId = _currentUserService.UserId;
            var entity = await _userRepository.GetByIdAsync(userId)
                ?? throw new DataNotFoundException(typeof(User), userId);

            var currentImage = entity.Image;
            _mapper.Map(dto, entity);

            if (_uploadFileService != null)
            {
                var entityWithImageAfter = (EntityWithImage)entity;
                var updateImageDto = (IUploadImageDTO)dto;

                entityWithImageAfter.Image = currentImage ?? string.Empty;
                if (updateImageDto.Image is { Length: > 0 } newImage)
                {
                    if (!string.IsNullOrWhiteSpace(currentImage))
                    {
                        await _uploadFileService.DeleteAsync(currentImage);
                    }

                    entityWithImageAfter.Image = await _uploadFileService.UploadAsync(newImage, typeof(User).Name);
                }
            }

            _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task CompleteProfileAsync(CreateUserWithRolePatientProfileDTO createPatientDto)
        {
            var authId = _currentUserService.AuthId;
            var stage = _currentUserService.Stage;

            if (Enum.TryParse<AuthStageEnum>(stage, true, out var stageEnum) && stageEnum == AuthStageEnum.PatientProfilePendingWithoutPhone
                && string.IsNullOrWhiteSpace(createPatientDto.Phone))
            {
                throw new ValidationFailureException(nameof(createPatientDto.Phone), "Phone number is required to complete profile");
            }

            var userDto = _mapper.Map<CreateUserDTO>(createPatientDto);
            userDto.Role ??= nameof(RoleEnum.Patient);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var user = await this.CreateAsync(userDto);

                var updateAuthAccountEvent = new UpdateAuthAccountWithUserIdEvent
                {
                    UserId = user.Id,
                    AuthId = authId,
                    Phone = createPatientDto?.Phone
                };

                await _messageBus.RequestAsync<UpdateAuthAccountWithUserIdEvent, UpdateAuthAccountWithUserIdContact>(updateAuthAccountEvent);
                await _messageBus.RequestAsync<CreatePatientEvent, CreatePatientContract>(new() { Id = user.Id });

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<int>> GetListUserIdByNameContainsAndRoleAsync(string? name, string? role)
        {
            if (string.IsNullOrWhiteSpace(role) || !Enum.TryParse<RoleEnum>(role, true, out var roleEnum))
            {
                return [];
            }

            var query = _userRepository.GetAllQueryable().Where(u => u.Role == roleEnum);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(u => u.Name!.Contains(name));
            }

            return await query.Select(u => u.Id).ToListAsync();
        }

        public override async Task<GetUserDTO> UpdateAsync(int id, UpdateUserDTO updateDTO)
        {
            if (updateDTO.Image == null)
            {
                var getStaff = await this.GetByIdAsync(id);
                updateDTO.Image = getStaff.Image;
            }

            return await base.UpdateAsync(id, updateDTO);
        }

        public async Task<List<int>> GetUserIdsByFilterAsync(UserFilterRequestDTO userFilterRequestDTO)
        {
            IQueryable<User> query = _userRepository.GetAllQueryable();

            if (!string.IsNullOrEmpty(userFilterRequestDTO.Name))
            {
                query = query.Where(x => x.Name != null && x.Name.Contains(userFilterRequestDTO.Name));
            }

            if (!string.IsNullOrEmpty(userFilterRequestDTO.Gender)
                && Enum.TryParse<GenderEnum>(userFilterRequestDTO.Gender, true, out var genderEnum))
            {
                query = query.Where(x => x.Gender == genderEnum);
            }

            if (!string.IsNullOrEmpty(userFilterRequestDTO.Role)
                && Enum.TryParse<RoleEnum>(userFilterRequestDTO.Role, true, out var roleEnum))
            {
                query = query.Where(x => x.Role == roleEnum);
            }

            return await query.Select(x => x.Id).ToListAsync();
        }

        public async Task<List<UserProfileDTO>> GetListUserDataByRoleAsync(string role)
        {
            if (!Enum.TryParse<RoleEnum>(role, true, out var roleEnum))
            {
                throw new InvalidDataException("Invalid role value");
            }

            var users = await _userRepository.GetAllQueryable().Where(u => u.Role == roleEnum).ToListAsync();
            return _mapper.Map<List<UserProfileDTO>>(users);
        }

        public async Task<List<UserEmailDTO>> GetUserEmailsByRolesAsync(List<string> roles, List<int> excludeUserIds)
        {
            var roleEnums = roles
                .Where(r => Enum.TryParse<RoleEnum>(r, true, out _))
                .Select(r => Enum.Parse<RoleEnum>(r, true))
                .ToList();

            if (roleEnums.Count == 0)
            {
                return [];
            }

            var query = _userRepository.GetAllQueryable()
                .Where(u => roleEnums.Contains(u.Role));

            if (excludeUserIds.Count > 0)
            {
                query = query.Where(u => !excludeUserIds.Contains(u.Id));
            }

            var users = await query.ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();
            var authContracts = await _messageBus.RequestAsync<
                GetListAuthDataByUserIdsEvent,
                GetListAuthDataByUserIdsContract>(
                new GetListAuthDataByUserIdsEvent { UserIds = userIds });

            var authDict = authContracts.Data.ToDictionary(a => a.UserId);

            return users.Select(u =>
            {
                authDict.TryGetValue(u.Id, out var auth);
                return new UserEmailDTO
                {
                    UserId = u.Id,
                    Name = u.Name,
                    Email = auth?.Email
                };
            }).ToList();
        }

        public async Task<List<GetUserDTO>> GetListUserDataByListRoleAsync(List<string> roles)
        {
            if (roles == null || !roles.Any())
            {
                return [];
            }

            var roleEnums = roles
                .Select(r => EnumUtil.TryParse(r, out RoleEnum role) ? role : (RoleEnum?)null)
                .Where(r => r.HasValue)
                .Select(r => r!.Value)
                .ToList();

            if (!roleEnums.Any())
            {
                return [];
            }

            var users = await _userRepository.GetAllQueryable()
                .Where(u => roleEnums.Contains(u.Role))
                .ToListAsync();

            return _mapper.Map<List<GetUserDTO>>(users);
        }

        public async Task<int> GetNewUsersCountByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var fromDateUtc = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var toDateUtc = new DateTime(toDate.Year, toDate.Month, toDate.Day, 0, 0, 0, DateTimeKind.Utc);
            var toDateExclusive = toDateUtc.AddDays(1);

            return await _userRepository.GetAllQueryable()
                .Where(user => user.CreationDate >= fromDateUtc && user.CreationDate < toDateExclusive)
                .CountAsync();
        }
    }
}
