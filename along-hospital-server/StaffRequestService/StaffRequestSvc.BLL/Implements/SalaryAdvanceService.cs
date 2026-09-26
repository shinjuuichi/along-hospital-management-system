using AutoMapper;
using MessageBroker.Contracts.PayrollContracts;
using MessageBroker.Events.PayrollEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using StaffRequestSvc.BLL.DTOs.SalaryAdvanceDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.BLL.StateMachines;
using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.Implements
{
    public class SalaryAdvanceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : BaseService<SalaryAdvance, CreateSalaryAdvanceDTO, UpdateSalaryAdvanceDTO, GetSalaryAdvanceDTO>(unitOfWork, mapper),
          ISalaryAdvanceService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private const double MaxAdvanceRate = 0.5;

        #region Overrides methods
        public override async Task<GetSalaryAdvanceDTO> CreateAsync(CreateSalaryAdvanceDTO createDTO)
        {
            var staffId = _currentUserService.UserId;

            await this.ValidateMaximumAdvanceAmountAsync(staffId, createDTO.Amount);
            await this.ValidateNoActiveAdvanceAsync(staffId);

            var salaryAdvance = _mapper.Map<SalaryAdvance>(createDTO);
            var stateMachine = new SalaryAdvanceStateMachine(salaryAdvance, staffId);

            if (_currentUserService.Role == RoleEnum.Manager)
            {
                stateMachine.Fire(SalaryAdvanceStatusEnum.Approved);
            }

            var result = await _repository.AddAsync(salaryAdvance);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }

        public override async Task<GetSalaryAdvanceDTO> UpdateAsync(int id, UpdateSalaryAdvanceDTO updateDTO)
        {
            var staffId = _currentUserService.UserId;

            var salaryAdvance = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(SalaryAdvance), id);

            if (salaryAdvance.Status != SalaryAdvanceStatusEnum.Pending)
            {
                throw new InvalidDataException("Salary advance can only be updated when status is Pending.");
            }

            if (staffId != salaryAdvance.CreatedBy)
            {
                throw new InvalidDataException("Invalid salary advance requester.");
            }

            await this.ValidateMaximumAdvanceAmountAsync(staffId, updateDTO.Amount);

            _mapper.Map(updateDTO, salaryAdvance);
            _repository.Update(salaryAdvance);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(salaryAdvance.Id);
        }
        #endregion

        #region Primary methods
        public async Task<PaginationResult<GetSalaryAdvanceDTO>> GetMyRequestsAsync(SalaryAdvanceFilterDTO filterDTO)
        {
            filterDTO.CreatedBy = _currentUserService.UserId;
            return await base.GetAllPaginatedAsync(filterDTO);
        }

        public async Task UpdateStatusAsync(int id, SalaryAdvanceStatusEnum newStatus)
        {
            var salaryAdvance = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(SalaryAdvance), id);

            this.ApplyStatusChange(salaryAdvance, newStatus);
            _repository.Update(salaryAdvance);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateListStatusAsync(List<int> ids, SalaryAdvanceStatusEnum newStatus)
        {
            if (ids.Count == 0)
            {
                throw new InvalidDataException("Salary advance ids cannot be empty.");
            }

            var salaryAdvances = await _repository.GetAllByIdsAsync(ids);

            foreach (var salaryAdvance in salaryAdvances)
            {
                this.ApplyStatusChange(salaryAdvance, newStatus);
            }

            _repository.UpdateRange(salaryAdvances);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<GetSalaryAdvanceDTO?> GetUndisbursedByStaffIdAsync(int staffId)
        {
            var salaryAdvance = await _repository
                .GetByConditionAsync(x => x.CreatedBy == staffId && x.Status == SalaryAdvanceStatusEnum.Approved);

            return _mapper.Map<GetSalaryAdvanceDTO>(salaryAdvance);
        }

        public async Task<GetSalaryAdvanceDTO?> MarkDisbursedByPayrollAsync(int staffId, int payrollId)
        {
            var salaryAdvance = await _repository.GetByConditionAsync(
                x => x.CreatedBy == staffId && x.Status == SalaryAdvanceStatusEnum.Approved)
                    ?? throw new DataNotFoundException(typeof(SalaryAdvance), $"No approved salary advance found for staff ID {staffId}.");

            var stateMachine = new SalaryAdvanceStateMachine(salaryAdvance, _currentUserService.UserId);
            if (!stateMachine.CanFire(SalaryAdvanceStatusEnum.Disbursed))
            {
                return _mapper.Map<GetSalaryAdvanceDTO>(salaryAdvance);
            }

            stateMachine.Fire(SalaryAdvanceStatusEnum.Disbursed);
            salaryAdvance.PayrollId = payrollId;

            _repository.Update(salaryAdvance);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(salaryAdvance.Id);
        }
        #endregion

        #region Helper methods
        private async Task ValidateNoActiveAdvanceAsync(int staffId)
        {
            var hasActiveRequest = await _repository.AnyAsync(x =>
                x.CreatedBy == staffId
                && (x.Status == SalaryAdvanceStatusEnum.Pending || x.Status == SalaryAdvanceStatusEnum.Approved));

            if (hasActiveRequest)
            {
                throw new InvalidDataException("You already have a salary advance request with Pending or Approved status.");
            }
        }

        private async Task ValidateMaximumAdvanceAmountAsync(int staffId, double amount)
        {
            var payrollContract = await _messageBus
                .RequestAsync<GetPayrollNetSalaryLatestByStaffIdEvent, GetPayrollNetSalaryLatestByStaffIDContract>(
                    new GetPayrollNetSalaryLatestByStaffIdEvent { StaffId = staffId });

            if (payrollContract.NetSalary <= 0)
            {
                throw new InvalidDataException("No latest payroll found. You cannot create a salary advance request yet.");
            }

            var maxAllowedAmount = payrollContract.NetSalary * MaxAdvanceRate;
            if (amount > maxAllowedAmount)
            {
                throw new InvalidDataException($"Salary advance amount cannot exceed 50% of your latest payroll net salary ({maxAllowedAmount}).");
            }
        }

        private void ApplyStatusChange(SalaryAdvance entity, SalaryAdvanceStatusEnum status)
        {
            var stateMachine = new SalaryAdvanceStateMachine(entity, _currentUserService.UserId);

            if (!stateMachine.CanFire(status))
            {
                throw new InvalidDataException($"Cannot change status from {entity.Status} to {status}.");
            }

            if (status == SalaryAdvanceStatusEnum.Cancelled && entity.CreatedBy != _currentUserService.UserId)
            {
                throw new ForbiddenException("You can only cancel your own salary advance request.");
            }

            if (status == SalaryAdvanceStatusEnum.Approved
                && _currentUserService.Role == RoleEnum.Accountant
                && entity.CreatedBy == _currentUserService.UserId)
            {
                throw new ForbiddenException("Accountant cannot approve their own salary advance request. A manager must approve this request.");
            }

            stateMachine.Fire(status);
        }
        #endregion
    }
}
