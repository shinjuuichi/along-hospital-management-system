using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using MessageBroker.Events.StaffRequestEvents;
using MessageBroker.Events.WorkScheduleEvents;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using StaffRequestSvc.BLL.DTOs;
using StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.BLL.StateMachines;
using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.BLL.Implements
{
    public class LeaveRequestService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus _bus,
        ICurrentUserService _currentUserService)
        : BaseService<LeaveRequest, CreateLeaveRequestDTO, UpdateLeaveRequestDTO, GetLeaveRequestDTO>(unitOfWork, mapper),
          ILeaveRequestService
    {
        public override async Task<GetLeaveRequestDTO> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            await this.PopulateLeaveRequestReferenceDataAsync([result]);
            return result;
        }

        public override async Task<PaginationResult<GetLeaveRequestDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO);
            await this.PopulateLeaveRequestReferenceDataAsync(result.Collection);
            return result;
        }

        public override async Task<GetLeaveRequestDTO> CreateAsync(CreateLeaveRequestDTO createDTO)
        {
            var leaveUnit = this.ParseLeaveUnit(createDTO.LeaveUnit);
            this.NormalizeLeaveDatesByUnit(createDTO, leaveUnit);
            var leaveRequestValidationDTO = _mapper.Map<LeaveRequestValidationDTO>(
                createDTO,
                opt => opt.Items[nameof(LeaveRequestValidationDTO.StaffId)] = _currentUserService.UserId);
            await this.ValidateLeaveScheduleAsync(leaveRequestValidationDTO);

            var entity = _mapper.Map<LeaveRequest>(createDTO);
            var isManagerRequest = _currentUserService.Role == RoleEnum.Manager;
            if (isManagerRequest)
            {
                var stateMachine = new LeaveRequestStateMachine(entity, _currentUserService.UserId);
                stateMachine.Fire(RequestStatusEnum.Approved);
            }

            var result = await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            if (isManagerRequest)
            {
                await this.PublishLeaveApprovedEventAsync(entity);
            }

            if (!isManagerRequest)
            {
                await this.SendCreatedEmailNotificationAsync(entity, "Created");
            }

            return await this.GetByIdAsync(result.Id);
        }

        public async Task<PaginationResult<GetLeaveRequestDTO>> GetMyRequestsAsync(LeaveRequestFilterDTO filterDTO)
        {
            filterDTO.CreatedBy = _currentUserService.UserId;
            var result = await base.GetAllPaginatedAsync(filterDTO);
            await this.PopulateLeaveRequestReferenceDataAsync(result.Collection);
            return result;
        }

        public async Task<GetLeaveRequestDTO> ChangeStatusAsync(int id, RequestStatusEnum action)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(LeaveRequest), id);

            var stateMachine = new LeaveRequestStateMachine(entity, _currentUserService.UserId);

            if (!stateMachine.CanFire(action))
            {
                throw new DataConflictException($"Cannot transition leave request from '{entity.Status}' to '{action}'");
            }

            switch (action)
            {
                case RequestStatusEnum.Canceled:
                    if (entity.CreatedBy != _currentUserService.UserId)
                    {
                        throw new ForbiddenException("You can only cancel your own leave requests");
                    }
                    break;

                case RequestStatusEnum.Approved:
                case RequestStatusEnum.Rejected:
                    if (entity.CreatedBy == _currentUserService.UserId)
                    {
                        throw new ForbiddenException($"You cannot {action.ToString().ToLower()} your own leave request");
                    }

                    await this.EnforceApprovalByManagerOnlyAsync(entity);

                    if (action == RequestStatusEnum.Approved)
                    {
                        var leaveRequestValidationDTO = _mapper.Map<LeaveRequestValidationDTO>(entity);
                        await this.ValidateLeaveScheduleAsync(leaveRequestValidationDTO);
                    }
                    break;
            }

            stateMachine.Fire(action);

            _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();

            if (action == RequestStatusEnum.Approved)
            {
                await this.PublishLeaveApprovedEventAsync(entity);
            }

            var actionName = action.ToString();
            if (action == RequestStatusEnum.Canceled)
            {
                await this.SendCreatedEmailNotificationAsync(entity, actionName);
            }
            else
            {
                await this.SendStatusEmailNotificationAsync(entity, actionName);
            }

            return await this.GetByIdAsync(entity.Id);
        }

        public async Task<List<ApprovedLeaveDTO>> GetApprovedLeavesByStaffsAndRangeAsync(
            GetApprovedLeavesByStaffsRangeDTO approvedLeavesByStaffsRangeDTO)
        {
            if (approvedLeavesByStaffsRangeDTO.StaffIds.Count == 0)
            {
                return [];
            }

            var leaveRequests = await _repository.GetAllAsync(l =>
                l.CreatedBy.HasValue &&
                approvedLeavesByStaffsRangeDTO.StaffIds.Contains(l.CreatedBy.Value) &&
                l.Status == RequestStatusEnum.Approved &&
                l.FromDate <= approvedLeavesByStaffsRangeDTO.ToDate &&
                l.ToDate >= approvedLeavesByStaffsRangeDTO.FromDate);

            return leaveRequests.Select(l => new ApprovedLeaveDTO
            {
                StaffId = l.CreatedBy!.Value,
                FromDate = l.FromDate,
                ToDate = l.ToDate,
                ShiftId = l.ShiftId,
                LeaveUnit = l.LeaveUnit.ToString()
            })
            .OrderBy(x => x.StaffId)
            .ThenBy(x => x.FromDate)
            .ToList();
        }

        #region Helper Methods
        private async Task EnforceApprovalByManagerOnlyAsync(LeaveRequest entity)
        {
            var usersContract = await _bus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
                new GetListUserDataByUserIdsEvent { UserIds = [entity.CreatedBy!.Value] });

            var requesterData = usersContract.Data.FirstOrDefault();
            if (requesterData is null)
            {
                return;
            }

            var isRequesterHr = string.Equals(requesterData.Role, nameof(RoleEnum.HR), StringComparison.OrdinalIgnoreCase);
            var isRequesterManager = string.Equals(requesterData.Role, nameof(RoleEnum.Manager), StringComparison.OrdinalIgnoreCase);

            if (isRequesterHr && _currentUserService.Role != RoleEnum.Manager)
            {
                throw new ForbiddenException("Only a Manager can approve or reject leave requests submitted by HR staff");
            }

            if (isRequesterManager && _currentUserService.Role != RoleEnum.Manager)
            {
                throw new ForbiddenException("Only a Manager can approve or reject leave requests submitted by other Managers");
            }
        }

        private async Task SendStatusEmailNotificationAsync(LeaveRequest entity, string action)
        {
            await _bus.PublishAsync(new SendStaffRequestStatusEmailEvent
            {
                StaffId = entity.CreatedBy!.Value,
                DeciderId = entity.DecidedBy,
                RequestType = "Leave Request",
                Action = action,
                Details = $"{entity.LeaveType} · {entity.FromDate:yyyy-MM-dd} to {entity.ToDate:yyyy-MM-dd}",
                Reason = entity.Reason
            });
        }

        private async Task SendCreatedEmailNotificationAsync(LeaveRequest entity, string action)
        {
            await _bus.PublishAsync(new SendWhenStaffRequestCreatedOrCancelledEmailEvent
            {
                StaffId = entity.CreatedBy!.Value,
                RequestType = "Leave Request",
                Action = action,
                Details = $"{entity.LeaveType} · {entity.FromDate:yyyy-MM-dd} to {entity.ToDate:yyyy-MM-dd}",
                Reason = entity.Reason
            });
        }

        private LeaveUnitEnum ParseLeaveUnit(string? leaveUnitValue)
        {
            if (string.IsNullOrWhiteSpace(leaveUnitValue))
            {
                throw new ValidationFailureException(nameof(CreateLeaveRequestDTO.LeaveUnit), "Leave unit is required");
            }

            if (!Enum.TryParse<LeaveUnitEnum>(leaveUnitValue.Trim(), true, out var leaveUnit))
            {
                throw new ValidationFailureException(nameof(CreateLeaveRequestDTO.LeaveUnit), "Leave unit must be Day or Shift");
            }

            return leaveUnit;
        }

        private async Task ValidateLeaveScheduleAsync(LeaveRequestValidationDTO leaveRequestValidationDTO)
        {
            var validateLeaveRequestEvent = _mapper.Map<ValidateLeaveRequestEvent>(leaveRequestValidationDTO);
            await _bus.RequestAsync<ValidateLeaveRequestEvent, ValidateLeaveRequestContract>(validateLeaveRequestEvent);
        }

        private async Task PublishLeaveApprovedEventAsync(LeaveRequest entity)
        {
            var leaveRequestValidationDTO = _mapper.Map<LeaveRequestValidationDTO>(entity);
            var leaveRequestApprovedEvent = _mapper.Map<LeaveRequestApprovedEvent>(leaveRequestValidationDTO);
            await _bus.PublishAsync(leaveRequestApprovedEvent);
        }

        private void NormalizeLeaveDatesByUnit(
            CreateLeaveRequestDTO createDTO,
            LeaveUnitEnum leaveUnit)
        {
            createDTO.LeaveUnit = leaveUnit.ToString();

            if (leaveUnit == LeaveUnitEnum.Day)
            {
                createDTO.ShiftId = null;
            }
            else if (leaveUnit == LeaveUnitEnum.Shift)
            {
                if (!createDTO.ShiftId.HasValue)
                {
                    throw new ValidationFailureException(nameof(CreateLeaveRequestDTO.ShiftId), "Shift is required for shift leave");
                }

                createDTO.ToDate = createDTO.FromDate;
            }
        }

        private async Task PopulateLeaveRequestReferenceDataAsync(List<GetLeaveRequestDTO> dtos)
        {
            if (dtos.Count == 0)
            {
                return;
            }

            await this.RequestValueForUserInfoDTOAsync(dtos);
            await this.RequestValueForShiftDTOAsync(dtos);
        }

        private async Task RequestValueForUserInfoDTOAsync(List<GetLeaveRequestDTO> dtos)
        {
            var dtoIds = dtos.Select(d => d.Id).ToList();
            var entityDict = await _repository.GetAllQueryable()
                .Where(e => dtoIds.Contains(e.Id))
                .ToDictionaryAsync(e => e.Id);

            var allUserIds = new HashSet<int>();
            foreach (var dto in dtos)
            {
                if (entityDict.TryGetValue(dto.Id, out var entity) && entity != null)
                {
                    if (entity.CreatedBy.HasValue)
                    {
                        allUserIds.Add(entity.CreatedBy.Value);
                    }

                    if (entity.DecidedBy.HasValue)
                    {
                        allUserIds.Add(entity.DecidedBy.Value);
                    }
                }
            }

            if (allUserIds.Count == 0)
            {
                return;
            }

            var usersContract = await _bus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
            new GetListUserDataByUserIdsEvent { UserIds = allUserIds.ToList() });

            var userDataDict = usersContract.Data.ToDictionary(u => u.UserId);

            foreach (var dto in dtos)
            {
                if (!entityDict.TryGetValue(dto.Id, out var entity) || entity == null)
                {
                    continue;
                }

                if (entity.CreatedBy.HasValue && userDataDict.TryGetValue(entity.CreatedBy.Value, out var staffData))
                {
                    dto.Staff = _mapper.Map<UserInfoDTO>(staffData);
                }

                if (entity.DecidedBy.HasValue && userDataDict.TryGetValue(entity.DecidedBy.Value, out var deciderData))
                {
                    dto.Decider = _mapper.Map<UserInfoDTO>(deciderData);
                }
            }
        }

        private async Task RequestValueForShiftDTOAsync(List<GetLeaveRequestDTO> dtos)
        {
            var shiftIds = dtos
                .Where(dto =>
                    dto.ShiftId.HasValue &&
                    string.Equals(dto.LeaveUnit, LeaveUnitEnum.Shift.ToString(), StringComparison.OrdinalIgnoreCase))
                .Select(dto => dto.ShiftId!.Value)
                .Distinct()
                .ToList();

            if (shiftIds.Count == 0)
            {
                return;
            }

            var shiftContract = await _bus.RequestAsync<GetListShiftsDataByIdsEvent, GetListShiftsDataByIdsContract>(
                new GetListShiftsDataByIdsEvent { Ids = shiftIds });
            var shiftDict = shiftContract.Data.ToDictionary(shift => shift.Id);

            foreach (var dto in dtos)
            {
                dto.ShiftName = null;
                dto.ShiftStartTime = null;
                dto.ShiftEndTime = null;

                if (!dto.ShiftId.HasValue ||
                    !string.Equals(dto.LeaveUnit, LeaveUnitEnum.Shift.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!shiftDict.TryGetValue(dto.ShiftId.Value, out var shift))
                {
                    continue;
                }

                dto.ShiftName = shift.Name;
                dto.ShiftStartTime = shift.StartTime;
                dto.ShiftEndTime = shift.EndTime;
            }
        }
        #endregion
    }
}
