using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.PaymentEvents;
using MessageBroker.Events.StaffEvents;
using MessageBroker.Events.StaffRequestEvents;
using MessageBroker.Events.UserEvents;
using Microsoft.EntityFrameworkCore;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.BLL.FilterDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.BLL.StateMachines;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using PayrollSvc.DAL.Models.Snapshot;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Extensions;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using System.Linq.Expressions;

namespace PayrollSvc.BLL.Implements
{
    public class PayrollService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IGlobalTaxConfigService globalTaxConfigService,
        IPayrollPolicyService payrollPolicyService,
        IAllowanceTypeService allowanceTypeService,
        IDeductionTypeService deductionTypeService,
        ITaxBracketService taxBracketService,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : BaseService<Payroll, CreatePayrollDTO, UpdatePayrollDTO, GetPayrollDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(Payroll.Allowances), nameof(Payroll.Deductions)]), IPayrollService
    {
        private readonly IGlobalTaxConfigService _globalTaxConfigService = globalTaxConfigService;
        private readonly IPayrollPolicyService _payrollPolicyService = payrollPolicyService;
        private readonly IAllowanceTypeService _allowanceTypeService = allowanceTypeService;
        private readonly IDeductionTypeService _deductionTypeService = deductionTypeService;
        private readonly ITaxBracketService _taxBracketService = taxBracketService;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        private const string PaymentStatusSuccess = "Success";

        private static readonly PayrollStatusEnum[] EditableStatuses = [PayrollStatusEnum.Draft, PayrollStatusEnum.Pending];

        #region Overide methods
        public override async Task<GetPayrollDTO> CreateAsync(CreatePayrollDTO createPayrollDto)
        {
            #region Check if the Payroll exists.
            var now = DateTime.UtcNow;
            int payrollMonth = now.Month;
            int payrollYear = now.Year;

            bool payrollExisted = await _repository.AnyAsync(p =>
                p.StaffId == createPayrollDto.StaffId &&
                p.Month == payrollMonth &&
                p.Year == payrollYear);

            if (payrollExisted)
            {
                throw new DataConflictException($"Payroll for staff {createPayrollDto.StaffId} in {payrollMonth}/{payrollYear} already exists.");
            }
            #endregion

            #region Fetch StaffContract, StaffProfile and SalaryAdvance data.
            var staffContractTask =
                _messageBus.RequestAsync<GetStaffContractByStaffIdEvent, GetStaffContractByStaffIdContract>(
                    new GetStaffContractByStaffIdEvent { StaffId = createPayrollDto.StaffId });
            var staffProfileTask =
                _messageBus.RequestAsync<GetStaffProfileEvent, GetStaffProfileContract>(
                    new GetStaffProfileEvent { StaffId = createPayrollDto.StaffId });
            var salaryAdvanceTask =
                _messageBus.RequestAsync<GetUndisbursedSalaryAdvanceByStaffIdEvent, GetUndisbursedSalaryAdvanceByStaffIdContract>(
                    new GetUndisbursedSalaryAdvanceByStaffIdEvent { StaffId = createPayrollDto.StaffId });

            await Task.WhenAll(staffContractTask, staffProfileTask, salaryAdvanceTask);

            var staffContractContract = staffContractTask.Result;
            var staffProfileContract = staffProfileTask.Result;
            var salaryAdvanceContract = salaryAdvanceTask.Result;
            #endregion

            #region Get resolved payroll policies and build allowances/deductions.
            var payroll = _mapper.Map<Payroll>(createPayrollDto);

            var resolvedPolicies = await _payrollPolicyService.GetResolvedPoliciesByStaffAsync(
                createPayrollDto.StaffId,
                DateOnly.FromDateTime(now));

            payroll.Allowances = await _allowanceTypeService.BuildAllowancesAsync(resolvedPolicies, createPayrollDto);
            payroll.Deductions = await _deductionTypeService.BuildDeductionsAsync(resolvedPolicies, createPayrollDto);
            #endregion

            #region Save snapshot entities
            var getGlobalTaxConfigDTO = await _globalTaxConfigService.GetCurrentConfigAsync();
            var getTaxBracketDTOs = await _taxBracketService.GetCurrentConfigAsync();

            payroll.GlobalTaxConfigSnapshot = _mapper.Map<GlobalTaxConfigSnapshot>(getGlobalTaxConfigDTO);
            payroll.RegionalWageSnapshot = _mapper.Map<RegionalWageSnapshot>(staffContractContract);
            payroll.StaffContractSnapshot = _mapper.Map<StaffContractSnapshot>(staffContractContract);
            payroll.StaffSnapshot = _mapper.Map<StaffSnapshot>(staffProfileContract);
            payroll.TaxBracketSnapshot = _mapper.Map<List<TaxBracketSnapshot>>(getTaxBracketDTOs);

            if (salaryAdvanceContract.Id > 0 && salaryAdvanceContract.Amount > 0)
            {
                payroll.SalaryAdvanceSnapshot = _mapper.Map<SalaryAdvanceSnapshot>(salaryAdvanceContract);
            }
            #endregion

            #region Savechanges
            var result = await _repository.AddAsync(payroll);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetPayrollDTO>(result);
            #endregion
        }

        public override async Task<GetPayrollDTO> UpdateAsync(int id, UpdatePayrollDTO updatePayrollDTO)
        {
            var payroll = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(Payroll), id);

            if (!EditableStatuses.Contains(payroll.Status))
            {
                throw new InvalidDataException($"Cannot modify payroll when status is {payroll.Status}.");
            }

            return await base.UpdateAsync(id, updatePayrollDTO);
        }
        #endregion

        #region Primary methods
        public override async Task<GetPayrollDTO> GetByIdAsync(int id)
        {
            var payroll = await base.GetByIdAsync(id);

            var userData = await _messageBus.RequestAsync<GetUserDataByUserIdEvent, GetUserDataByUserIdContract>(
                new GetUserDataByUserIdEvent { UserId = payroll.StaffId });

            _mapper.Map(userData, payroll);

            return payroll;
        }

        public override async Task<List<GetPayrollDTO>> GetAllAsync()
        {
            var payrolls = await base.GetAllAsync();
            return await this.CombinedUserAndPayrollDataAsync(payrolls);
        }

        public override async Task<List<GetPayrollDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var payrolls = await base.GetAllByIdsAsync(ids);
            return await this.CombinedUserAndPayrollDataAsync(payrolls);
        }

        public override async Task<PaginationResult<GetPayrollDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            Expression<Func<Payroll, bool>> filterExpr = p => true;
            var payrollFilterDTO = (PayrollFilterDTO)filterDTO;

            filterExpr = await this.ApplyPayrollFilterAsync(filterExpr, payrollFilterDTO);

            var (total, payrolls) = await _repository.GetAllPaginatedAsync(
                filterExpr,
                payrollFilterDTO.Filter,
                payrollFilterDTO.Sort,
                payrollFilterDTO.Page,
                payrollFilterDTO.PageSize,
                _includes);

            var payrollDtos = _mapper.Map<List<GetPayrollDTO>>(payrolls);
            var combined = await this.CombinedUserAndPayrollDataAsync(payrollDtos);

            return new PaginationResult<GetPayrollDTO>(total, payrollFilterDTO.PageSize, combined);
        }

        public async Task UpdatePayrollStatusAsync(List<int> ids, PayrollStatusEnum payrollStatus)
        {
            if (ids.Count == 0)
            {
                throw new InvalidDataException("Ids cannot be empty.");
            }

            var payrollIds = ids.Distinct().ToList();
            var payrolls = await _repository.GetAllAsync(x => payrollIds.Contains(x.Id));

            this.ApplyPayrollStatusRange(payrolls, payrollStatus);

            if (payrollStatus == PayrollStatusEnum.Approved)
            {
                foreach (var payroll in payrolls)
                {
                    if (string.IsNullOrWhiteSpace(payroll.StaffSnapshot?.BankCode))
                    {
                        throw new InvalidDataException("Bank code is required to create SePay payment.");
                    }

                    if (string.IsNullOrWhiteSpace(payroll.StaffSnapshot?.AccountNumber))
                    {
                        throw new InvalidDataException("Account number is required to create SePay payment.");
                    }

                    var userData = await _messageBus.RequestAsync<GetUserDataByUserIdEvent, GetUserDataByUserIdContract>(
                        new GetUserDataByUserIdEvent { UserId = payroll.StaffId });

                    var createPaymentEvent = new CreatePaymentEventForPayroll
                    {
                        Description = $"Payslip of {userData.Name} {payroll.Month} {payroll.Year}",
                        BankCode = payroll.StaffSnapshot.BankCode,
                        AccountNumber = payroll.StaffSnapshot.AccountNumber,
                        PaymentEventItems =
                        [
                            new PaymentEventItem
                            {
                                ServiceName = "Salary Payment",
                                Quantity = 1,
                                UnitPrice = payroll.NetSalary
                            }
                        ]
                    };

                    var paymentResult = await _messageBus.RequestAsync<CreatePaymentEventForPayroll, CreatePaymentForPayrollContract>(createPaymentEvent);
                    payroll.QrCode = paymentResult.PaymentUrl;
                    payroll.TransactionId = paymentResult.TransactionId;
                }
            }

            _repository.UpdateRange(payrolls);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task HandlePaymentStatusChangedAsync(PaymentStatusChangedDTO paymentStatusChangedDTO)
        {
            if (!string.Equals(paymentStatusChangedDTO.PaymentStatus, PaymentStatusSuccess, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var payroll = await _repository.GetByConditionAsync(x => x.TransactionId == paymentStatusChangedDTO.TransactionId)
                ?? throw new DataNotFoundException($"{nameof(Payroll)} with transaction id {paymentStatusChangedDTO.TransactionId} was not found.");

            if (payroll.Status == PayrollStatusEnum.Paid)
            {
                return;
            }

            this.ApplyPayrollStatusOrThrow(payroll, PayrollStatusEnum.Paid);

            _repository.Update(payroll);
            await _unitOfWork.SaveChangeAsync();

            if (payroll.SalaryAdvanceSnapshot is not null && payroll.SalaryAdvanceSnapshot.Amount > 0)
            {
                await _messageBus.PublishAsync(
                    new MarkSalaryAdvanceDisbursedByPayrollEvent
                    {
                        StaffId = payroll.StaffId,
                        PayrollId = payroll.Id
                    });
            }
        }

        public async Task TerminatePayrollsByStaffIdsAsync(List<int> staffIds)
        {
            if (staffIds.Count == 0)
            {
                return;
            }

            var payrolls = await _repository.GetAllAsync(x => staffIds.Contains(x.StaffId));
            if (payrolls.Count == 0)
            {
                return;
            }

            this.ApplyPayrollStatusRange(payrolls, PayrollStatusEnum.Terminated);

            _repository.UpdateRange(payrolls);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<double> GetLatestNetSalaryByStaffIdAsync(int staffId)
        {
            return await _repository.GetAllQueryable()
                .Where(x => x.StaffId == staffId)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.Id)
                .Select(x => x.NetSalary)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Private helpers
        private async Task<Expression<Func<Payroll, bool>>> ApplyPayrollFilterAsync(
            Expression<Func<Payroll, bool>> filterExpr,
            PayrollFilterDTO payrollFilterDTO)
        {
            if (payrollFilterDTO.Month.HasValue)
            {
                filterExpr = filterExpr.And(p => p.Month == payrollFilterDTO.Month.Value);
            }

            if (payrollFilterDTO.Year.HasValue)
            {
                filterExpr = filterExpr.And(p => p.Year == payrollFilterDTO.Year.Value);
            }

            if (!string.IsNullOrWhiteSpace(payrollFilterDTO.Status))
            {
                if (!EnumUtil.TryParse<PayrollStatusEnum>(payrollFilterDTO.Status, out var payrollStatus))
                {
                    throw new InvalidDataException("Invalid payroll status.");
                }

                filterExpr = filterExpr.And(p => p.Status == payrollStatus);
            }

            var hasUserFilter = !string.IsNullOrWhiteSpace(payrollFilterDTO.Name);

            if (!hasUserFilter)
            {
                return filterExpr;
            }

            var userEvent = _mapper.Map<PayrollFilterDTO, GetListUserIdByFilterUserEntityEvent>(payrollFilterDTO);
            var userResult = await _messageBus.RequestAsync<GetListUserIdByFilterUserEntityEvent, GetListUserIdByFilterUserEntityContract>(userEvent);

            var userIds = userResult.UserIds;
            if (userIds.Count == 0)
            {
                return _ => false;
            }

            return filterExpr.And(p => userIds.Contains(p.StaffId));
        }

        private void ApplyPayrollStatusRange(List<Payroll> payrolls, PayrollStatusEnum payrollStatus)
        {
            foreach (var payroll in payrolls)
            {
                this.ApplyPayrollStatusOrThrow(payroll, payrollStatus);
            }
        }

        private void ApplyPayrollStatusOrThrow(Payroll payroll, PayrollStatusEnum payrollStatus)
        {
            var payrollStatusStateMachine = new PayrollStatusStateMachine(payroll);
            if (!payrollStatusStateMachine.CanFire(payrollStatus))
            {
                throw new InvalidDataException($"Cannot change payroll status from {payroll.Status} to {payrollStatus}.");
            }

            payrollStatusStateMachine.Fire(payrollStatus);
        }

        private async Task<List<GetPayrollDTO>> CombinedUserAndPayrollDataAsync(List<GetPayrollDTO> payrolls)
        {
            var staffIds = payrolls.Select(p => p.StaffId).Distinct().ToList();
            if (staffIds.Count == 0)
            {
                return payrolls;
            }

            var userContracts = await _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
                new GetListUserDataByUserIdsEvent { UserIds = staffIds });

            var userDict = userContracts.Data.ToDictionary(u => u.UserId);

            foreach (var payroll in payrolls)
            {
                if (userDict.TryGetValue(payroll.StaffId, out var userData))
                {
                    _mapper.Map(userData, payroll);
                }
            }

            return payrolls;
        }
        #endregion
    }
}
