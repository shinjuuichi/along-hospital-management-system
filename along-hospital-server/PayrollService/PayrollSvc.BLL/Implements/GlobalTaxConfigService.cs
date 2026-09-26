using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PayrollSvc.BLL.DTOs.GlobalTaxConfigDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace PayrollSvc.BLL.Implements
{
    public class GlobalTaxConfigService(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService<GlobalTaxConfig, CreateGlobalTaxConfigDTO, UpdateGlobalTaxConfigDTO, GetGlobalTaxConfigDTO>(unitOfWork, mapper), IGlobalTaxConfigService
    {
        public async Task<GetGlobalTaxConfigDTO> GetCurrentConfigAsync()
        {
            var currentConfig = await _repository.GetAllQueryable().FirstOrDefaultAsync()
                ?? throw new DataNotFoundException("No global tax configuration found.");

            return _mapper.Map<GetGlobalTaxConfigDTO>(currentConfig);
        }
    }
}