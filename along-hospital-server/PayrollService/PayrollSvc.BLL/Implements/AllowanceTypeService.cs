using AutoMapper;
using PayrollSvc.BLL.DTOs.AllowanceTypeDTOs;
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
    public class AllowanceTypeService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<AllowanceType, CreateAllowanceTypeDTO, UpdateAllowanceTypeDTO, GetAllowanceTypeDTO>(unitOfWork, mapper), IAllowanceTypeService
    {
        #region Overrides
        public override async Task<PaginationResult<GetAllowanceTypeDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var (total, entities) = await _repository.GetAllPaginatedAsync(
                a => a.IsSystemGenerated == false,
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var allowanceTypeDTOs = _mapper.Map<List<GetAllowanceTypeDTO>>(entities);
            return new PaginationResult<GetAllowanceTypeDTO>(total, filterDTO.PageSize, allowanceTypeDTOs);
        }

        public override async Task<List<GetAllowanceTypeDTO>> GetAllAsync()
        {
            var allowanceTypes = await _repository.GetAllAsync(a => a.IsSystemGenerated == false, _includes);
            return _mapper.Map<List<GetAllowanceTypeDTO>>(allowanceTypes);
        }
        #endregion

        #region Primary method
        public async Task<List<Allowance>> BuildAllowancesAsync(List<PayrollPolicy> resolvedPolicies, CreatePayrollDTO createPayrollDTO)
        {
            List<Allowance> allowances = [];
            var allowanceTypes = await _repository.GetAllAsync();
            var allowanceTypeDTOs = _mapper.Map<List<GetAllowanceTypeDTO>>(allowanceTypes);

            var allowanceTypeMap = allowanceTypeDTOs.ToDictionary(x => x.Id);

            // 1) Generate automatic allowances from System types.
            var systemAllowanceTypeIds = allowanceTypeDTOs
                .Where(allowanceTypeDTO => allowanceTypeDTO.IsSystemGenerated)
                .Select(x => x.Id)
                .ToList();

            foreach (var allowanceTypeId in systemAllowanceTypeIds)
            {
                var getAllowanceTypeDTO = allowanceTypeMap[allowanceTypeId];

                int quantity = this.ResolveQuantity(getAllowanceTypeDTO.AllowanceQuantitySourceEnum, createPayrollDTO);
                if (quantity <= 0)
                {
                    continue;
                }

                allowances.Add(new Allowance
                {
                    PayrollPolicyId = null,
                    AllowanceTypeId = allowanceTypeId,
                    AllowanceTypeName = getAllowanceTypeDTO.Name!,
                    UnitPrice = getAllowanceTypeDTO.Price,
                    Quantity = quantity,
                    IsTaxable = getAllowanceTypeDTO.IsTaxable,
                });
            }


            var allowancePolicies = resolvedPolicies.Where(p => p.AllowanceTypeId.HasValue).ToList();

            if (allowancePolicies.Count == 0)
            {
                return allowances;
            }

            foreach (var policy in allowancePolicies)
            {
                int allowanceTypeId = policy.AllowanceTypeId!.Value;
                if (!allowanceTypeMap.TryGetValue(allowanceTypeId, out var getAllowanceTypeDTO))
                {
                    throw new DataNotFoundException(typeof(AllowanceType), allowanceTypeId);
                }

                int quantity = this.ResolveQuantity(getAllowanceTypeDTO.AllowanceQuantitySourceEnum, createPayrollDTO);
                if (quantity <= 0)
                {
                    continue;
                }

                allowances.Add(new Allowance
                {
                    PayrollPolicyId = policy.Id,
                    AllowanceTypeId = allowanceTypeId,
                    AllowanceTypeName = getAllowanceTypeDTO.Name!,
                    UnitPrice = getAllowanceTypeDTO.Price,
                    Quantity = quantity,
                    IsTaxable = getAllowanceTypeDTO.IsTaxable,
                });
            }

            return allowances;
        }
        #endregion

        #region Method Helpers
        private int ResolveQuantity(string? sourceValue, CreatePayrollDTO createPayrollDTO)
        {
            if (!Enum.TryParse<AllowanceQuantitySourceEnum>(sourceValue, true, out var sourceEnum))
            {
                return 1;
            }

            return sourceEnum switch
            {
                AllowanceQuantitySourceEnum.None => 1,
                AllowanceQuantitySourceEnum.OvertimeMinutes => createPayrollDTO.OvertimeMinutes,
                _ => 1,
            };
        }
        #endregion
    }
}
