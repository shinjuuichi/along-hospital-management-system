using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using StaffSvc.BLL.DTOs.StaffContractDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.BLL.StateMachines;
using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class StaffContractService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadFileService uploadFileService,
        IMessageBus messageBus)
        : BaseService<StaffContract, CreateStaffContractDTO, UpdateStaffContractDTO, GetStaffContractDTO>(
            unitOfWork, mapper, uploadFileService, includes: [nameof(StaffContract.Staff)]), IStaffContractService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public override async Task<GetStaffContractDTO> GetByIdAsync(int id)
        {
            var dto = await base.GetByIdAsync(id);
            await this.AddStaffNamesAsync([dto]);
            return dto;
        }

        public override async Task<PaginationResult<GetStaffContractDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO);
            await this.AddStaffNamesAsync(result.Collection);
            return result;
        }

        public async Task<GetStaffContractDTO> GetActiveContractByStaffIdAsync(int staffId)
        {
            var entity = await _repository.GetByConditionAsync(
                x => x.StaffId == staffId && x.Status == StaffContractStatusEnum.Active,
                includes: [nameof(StaffContract.Staff), nameof(StaffContract.RegionalWage)])
                    ?? throw new DataNotFoundException($"Active contract for staff {staffId} was not found.");

            var dto = _mapper.Map<GetStaffContractDTO>(entity);
            await this.AddStaffNamesAsync([dto]);
            return dto;
        }

        public override async Task<GetStaffContractDTO> CreateAsync(CreateStaffContractDTO createDTO)
        {
            var existingActiveContract = await _repository.AnyAsync(
                x => x.StaffId == createDTO.StaffId && x.Status == StaffContractStatusEnum.Active);

            if (existingActiveContract)
            {
                throw new InvalidDataException("Staff already has an active contract.");
            }

            var entity = _mapper.Map<StaffContract>(createDTO);
            entity.ContractCode = this.GenerateContractCode(createDTO.ContractType);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetStaffContractDTO>(entity);
        }

        public async Task UpdateStatusAsync(int id, StaffContractStatusEnum newStatus)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(StaffContract), id);

            this.HandleChangeStatus(entity, newStatus);

            _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task RenewAsync(int id, RenewStaffContractDTO renewDTO)
        {
            var oldContract = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(StaffContract), id);

            var createDto = new CreateStaffContractDTO
            {
                ContractType = oldContract.ContractType.ToString(),
                StartDate = renewDTO.StartDate,
                EndDate = renewDTO.EndDate,
                HourlyRate = oldContract.HourlyRate,
                WorkingHoursPerWeek = oldContract.WorkingHoursPerWeek,
                InsuranceSalaryRate = oldContract.InsuranceSalaryRate,
                StaffId = oldContract.StaffId,
                RegionalWageId = oldContract.RegionalWageId,
            };

            await this.CreateAsync(createDto);
        }

        public async Task<GetStaffContractDTO> SignContractAsync(int id, SignStaffContractDTO signDTO)
        {
            string? signatureImageUrl = null;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var entity = await _repository.GetByIdAsync(id)
                    ?? throw new DataNotFoundException(typeof(StaffContract), id);

                if (!string.IsNullOrEmpty(entity.SignatureImage))
                {
                    throw new InvalidDataException("Contract is already signed.");
                }

                signatureImageUrl = await _uploadFileService!.UploadAsync(
                    signDTO.SignatureImageFile, nameof(StaffContract));

                entity.SignatureImage = signatureImageUrl;
                entity.SignedDate = DateOnly.FromDateTime(DateTime.UtcNow);

                _repository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<GetStaffContractDTO>(entity);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                if (!string.IsNullOrEmpty(signatureImageUrl))
                {
                    await _uploadFileService!.DeleteAsync(signatureImageUrl);
                }

                throw;
            }
        }

        public async Task ExpireContractsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expiredContracts = await _repository.GetAllAsync(
                x => x.Status == StaffContractStatusEnum.Active
                     && x.EndDate.HasValue
                     && x.EndDate.Value < today);

            if (expiredContracts.Count == 0)
            {
                return;
            }

            foreach (var contract in expiredContracts)
            {
                this.HandleChangeStatus(contract, StaffContractStatusEnum.Expired);
            }

            _repository.UpdateRange(expiredContracts);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task SendContractExpiringNotificationsAsync(int daysBeforeExpiration = 30)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expirationDate = today.AddDays(daysBeforeExpiration);

            var expiringContracts = await _repository.GetAllAsync(
                x => x.Status == StaffContractStatusEnum.Active
                     && x.EndDate.HasValue
                     && x.EndDate.Value >= today
                     && x.EndDate.Value <= expirationDate,
                includes: _includes);

            if (expiringContracts.Count == 0)
            {
                return;
            }

            foreach (var contract in expiringContracts)
            {
                if (contract.Staff == null)
                {
                    continue;
                }

                var staffUserId = contract.StaffId;

                GetUserDataByUserIdContract? staffData = null;
                try
                {
                    var getUserDataContract = await _messageBus.RequestAsync<GetUserDataByUserIdEvent, GetUserDataByUserIdContract>(
                        new() { UserId = staffUserId });
                    staffData = getUserDataContract;
                }
                catch
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(staffData?.Email))
                {
                    continue;
                }

                var daysRemaining = contract.EndDate!.Value.DayNumber - today.DayNumber;

                await _messageBus.PublishAsync(new SendContractExpiringEmailEvent
                {
                    Email = staffData.Email,
                    StaffName = staffData.Name ?? "Staff",
                    ContractEndDate = contract.EndDate.Value,
                    DaysRemaining = daysRemaining
                });
            }
        }

        private void HandleChangeStatus(StaffContract entity, StaffContractStatusEnum newStatus)
        {
            var stateMachine = new StaffContractStateMachine(entity);

            if (!stateMachine.CanFire(newStatus))
            {
                throw new InvalidDataException($"Cannot transition contract from '{entity.Status}' to '{newStatus}'");
            }

            try
            {
                stateMachine.Fire(newStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change contract status: {e.Message}");
            }
        }

        private async Task AddStaffNamesAsync(List<GetStaffContractDTO> dtos)
        {
            var staffIds = dtos.Select(d => d.StaffId).Distinct().ToList();
            if (staffIds.Count == 0)
            {
                return;
            }

            var getListUserDataContract = await _messageBus
                .RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
                    new() { UserIds = staffIds });

            var userDict = getListUserDataContract.Data.ToDictionary(u => u.UserId, u => u);

            foreach (var dto in dtos)
            {
                if (userDict.TryGetValue(dto.StaffId, out var user))
                {
                    dto.StaffName = user.Name;
                    dto.StaffImage = user.Image;
                }
            }
        }

        private string GenerateContractCode(string? contractType)
        {
            var prefix = "HD";

            if (!string.IsNullOrWhiteSpace(contractType))
            {
                var letters = contractType
                    .ToUpperInvariant()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(w => w[0])
                    .Take(2)
                    .ToArray();

                if (letters.Length > 0)
                {
                    prefix = new string(letters);
                }
            }

            var randomNumber = Random.Shared.Next(100, 10000);

            return $"{prefix}{randomNumber}";
        }
    }
}
