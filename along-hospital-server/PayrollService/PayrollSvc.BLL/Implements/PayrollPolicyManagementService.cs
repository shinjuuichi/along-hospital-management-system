using AutoMapper;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.StaffEvents;
using PayrollSvc.BLL.DTOs.PayrollPolicyDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace PayrollSvc.BLL.Implements
{
    public class PayrollPolicyManagementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAllowanceTypeService allowanceTypeService,
        IDeductionTypeService deductionTypeService,
        IMessageBus messageBus)
        : BaseService<PayrollPolicy, CreatePayrollPolicyDTO, UpdatePayrollPolicyDTO, GetPayrollPolicyDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(PayrollPolicy.AllowanceType), nameof(PayrollPolicy.DeductionType), nameof(PayrollPolicy.PayrollPolicyStaffs)]), IPayrollPolicyService
    {
        private readonly IAllowanceTypeService _allowanceTypeService = allowanceTypeService;
        private readonly IDeductionTypeService _deductionTypeService = deductionTypeService;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IGenericRepository<PayrollPolicyStaff> _payrollPolicyStaffRepository = unitOfWork.Repository<PayrollPolicyStaff>();

        #region Overrides methods
        public override async Task<GetPayrollPolicyDTO> CreateAsync(CreatePayrollPolicyDTO createPayrollPolicyDTO)
        {
            var staffIds = await this.ResolveActiveStaffIdsAsync(createPayrollPolicyDTO.StaffIds);
            createPayrollPolicyDTO.StaffIds = staffIds.Distinct().ToList();
            var payrollPolicy = _mapper.Map<PayrollPolicy>(createPayrollPolicyDTO);

            this.ValidatePayrollPolicy(payrollPolicy);
            await this.EnsurePolicyTypeIsUserDefinedAsync(payrollPolicy);
            await this.EnsureNoCollisionAsync(payrollPolicy, staffIds);

            var result = await _repository.AddAsync(payrollPolicy);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }

        public override async Task<GetPayrollPolicyDTO> UpdateAsync(int id, UpdatePayrollPolicyDTO updatePayrollPolicyDTO)
        {
            var payrollPolicy = await _repository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(PayrollPolicy), id);

            _payrollPolicyStaffRepository.RemoveRange(payrollPolicy.PayrollPolicyStaffs.ToList());

            var staffIds = await this.ResolveActiveStaffIdsAsync(updatePayrollPolicyDTO.StaffIds);
            updatePayrollPolicyDTO.StaffIds = staffIds.Distinct().ToList();
            _mapper.Map(updatePayrollPolicyDTO, payrollPolicy);

            this.ValidatePayrollPolicy(payrollPolicy);
            await this.EnsurePolicyTypeIsUserDefinedAsync(payrollPolicy);
            await this.EnsureNoCollisionAsync(payrollPolicy, staffIds, id);

            var result = _repository.Update(payrollPolicy);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }
        #endregion

        #region Primary methods
        public async Task<List<PayrollPolicy>> GetResolvedPoliciesByStaffAsync(int staffId, DateOnly payrollDate)
        {
            var resolvedPolicies = await _repository.GetAllAsync(
                p => p.PayrollPolicyStatus == PayrollPolicyStatus.Active
                     && p.StartDate <= payrollDate
                     && p.EndDate >= payrollDate
                     && p.PayrollPolicyStaffs.Any(s => s.StaffId == staffId));

            if (resolvedPolicies.Count == 0)
            {
                return [];
            }

            return resolvedPolicies;
        }
        #endregion

        #region Helper methods
        private async Task EnsureNoCollisionAsync(PayrollPolicy payrollPolicy, List<int> staffIds, int? excludePolicyId = null)
        {
            if (payrollPolicy.PayrollPolicyStatus != PayrollPolicyStatus.Active)
            {
                return;
            }
            int excludeId = excludePolicyId ?? 0;

            List<PayrollPolicy> overlappingPolicies;

            if (payrollPolicy.AllowanceTypeId.HasValue)
            {
                int allowanceTypeId = payrollPolicy.AllowanceTypeId.Value;

                overlappingPolicies = await _repository.GetAllAsync(p =>
                    p.Id != excludeId
                    && p.PayrollPolicyStatus == PayrollPolicyStatus.Active
                    && p.StartDate <= payrollPolicy.EndDate
                    && p.EndDate >= payrollPolicy.StartDate
                    && p.AllowanceTypeId == allowanceTypeId
                    && p.PayrollPolicyStaffs.Any(s => staffIds.Contains(s.StaffId)));
            }
            else if (payrollPolicy.DeductionTypeId.HasValue)
            {
                int deductionTypeId = payrollPolicy.DeductionTypeId.Value;

                overlappingPolicies = await _repository.GetAllAsync(p =>
                    p.Id != excludeId
                    && p.PayrollPolicyStatus == PayrollPolicyStatus.Active
                    && p.StartDate <= payrollPolicy.EndDate
                    && p.EndDate >= payrollPolicy.StartDate
                    && p.DeductionTypeId == deductionTypeId
                    && p.PayrollPolicyStaffs.Any(s => staffIds.Contains(s.StaffId)));
            }
            else
            {
                throw new InvalidDataException("Either AllowanceTypeId or DeductionTypeId must be provided.");
            }

            if (overlappingPolicies.Count > 0)
            {
                throw new DataConflictException("Policy conflicts with an existing policy of the same type for the same staff in the same period.");
            }
        }

        private async Task EnsurePolicyTypeIsUserDefinedAsync(PayrollPolicy payrollPolicy)
        {
            if (payrollPolicy.AllowanceTypeId.HasValue)
            {
                int allowanceTypeId = payrollPolicy.AllowanceTypeId.Value;
                var allowanceType = await _allowanceTypeService.GetByIdAsync(allowanceTypeId);

                if (allowanceType.IsSystemGenerated)
                {
                    throw new InvalidDataException($"Payroll policy does not support system allowance type");
                }

                return;
            }

            int deductionTypeId = payrollPolicy.DeductionTypeId!.Value;
            var deductionType = await _deductionTypeService.GetByIdAsync(deductionTypeId);

            if (deductionType.IsSystemGenerated)
            {
                throw new InvalidDataException($"Payroll policy does not support system deduction type");
            }
        }

        private void ValidatePayrollPolicy(PayrollPolicy payrollPolicy)
        {
            if (payrollPolicy.AllowanceTypeId.HasValue == payrollPolicy.DeductionTypeId.HasValue)
            {
                throw new InvalidDataException("A payroll policy must have either an Allowance Type or a Deduction Type, but not both.");
            }
        }

        private async Task<List<int>> ResolveActiveStaffIdsAsync(List<int> staffIds)
        {
            var contract = await _messageBus.RequestAsync<GetActiveStaffIdsByIdsEvent, GetActiveStaffIdsByIdsContract>(
                new GetActiveStaffIdsByIdsEvent { StaffIds = staffIds });
            return contract.StaffIds;
        }
        #endregion
    }
}
