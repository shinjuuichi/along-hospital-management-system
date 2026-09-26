using AutoMapper;
using PayrollSvc.BLL.DTOs.DeductionTypeDTOs;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace PayrollSvc.BLL.Implements
{
    public class DeductionTypeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<DeductionType, CreateDeductionTypeDTO, UpdateDeductionTypeDTO, GetDeductionTypeDTO>(unitOfWork, mapper), IDeductionTypeService
    {
        #region Overrides
        public override async Task<PaginationResult<GetDeductionTypeDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var (total, entities) = await _repository.GetAllPaginatedAsync(
                a => a.IsSystemGenerated == false,
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var getDeductionTypeDTOs = _mapper.Map<List<GetDeductionTypeDTO>>(entities);
            return new PaginationResult<GetDeductionTypeDTO>(total, filterDTO.PageSize, getDeductionTypeDTOs);
        }

        public override async Task<List<GetDeductionTypeDTO>> GetAllAsync()
        {
            var deductionTypes = await _repository.GetAllAsync(a => a.IsSystemGenerated == false);
            return _mapper.Map<List<GetDeductionTypeDTO>>(deductionTypes);
        }
        #endregion

        #region Primary methods
        public async Task<List<Deduction>> BuildDeductionsAsync(List<PayrollPolicy> resolvedPolicies, CreatePayrollDTO createPayrollDTO)
        {
            List<Deduction> deductions = [];
            var deductionType = await _repository.GetAllAsync();
            var deductionTypeDTOs = _mapper.Map<List<GetDeductionTypeDTO>>(deductionType);

            var deductionTypeMap = deductionTypeDTOs.ToDictionary(x => x.Id);

            // 1) Generate automatic deductions from System types.
            var systemDeductionTypeIds = deductionTypeDTOs
                .Where(typeDto => typeDto.IsSystemGenerated)
                .Select(x => x.Id)
                .ToList();

            foreach (var deductionTypeId in systemDeductionTypeIds)
            {
                var getDeductionTypeDTO = deductionTypeMap[deductionTypeId];

                int quantity = this.ResolveQuantity(getDeductionTypeDTO.DeductionQuantitySourceEnum, createPayrollDTO);
                if (quantity <= 0)
                {
                    continue;
                }

                deductions.Add(new Deduction
                {
                    PayrollPolicyId = null,
                    DeductionTypeId = deductionTypeId,
                    DeductionTypeName = getDeductionTypeDTO.Name!,
                    UnitPrice = getDeductionTypeDTO.Price,
                    Quantity = quantity,
                    IsTaxable = getDeductionTypeDTO.IsTaxable,
                });
            }

            var deductionPolicies = resolvedPolicies.Where(p => p.DeductionTypeId.HasValue).ToList();

            if (deductionPolicies.Count == 0)
            {
                return deductions;
            }

            foreach (var policy in deductionPolicies)
            {
                int deductionTypeId = policy.DeductionTypeId!.Value;
                if (!deductionTypeMap.TryGetValue(deductionTypeId, out var getDeductionTypeDTO))
                {
                    throw new DataNotFoundException(typeof(DeductionType), deductionTypeId);
                }

                int quantity = this.ResolveQuantity(getDeductionTypeDTO.DeductionQuantitySourceEnum, createPayrollDTO);
                if (quantity <= 0)
                {
                    continue;
                }

                deductions.Add(new Deduction
                {
                    PayrollPolicyId = policy.Id,
                    DeductionTypeId = deductionTypeId,
                    DeductionTypeName = getDeductionTypeDTO.Name!,
                    UnitPrice = getDeductionTypeDTO.Price,
                    Quantity = quantity,
                    IsTaxable = getDeductionTypeDTO.IsTaxable,
                });
            }

            return deductions;
        }
        #endregion

        #region Helper methods
        private int ResolveQuantity(string? sourceValue, CreatePayrollDTO createPayrollDTO)
        {
            if (!Enum.TryParse<DeductionQuantitySourceEnum>(sourceValue, true, out var sourceEnum))
            {
                return 1;
            }

            return sourceEnum switch
            {
                DeductionQuantitySourceEnum.None => 1,
                DeductionQuantitySourceEnum.LateMinutes => createPayrollDTO.LateMinutes,
                DeductionQuantitySourceEnum.EarlyLeaveMinutes => createPayrollDTO.EarlyLeaveMinutes,
                _ => 1,
            };
        }
        #endregion
    }
}
