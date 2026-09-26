using AutoMapper;
using PayrollSvc.BLL.DTOs.TaxBracketDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace PayrollSvc.BLL.Implements
{
    public class TaxBracketService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<TaxBracket, CreateTaxBracketDTO, UpdateTaxBracketDTO, GetTaxBracketDTO>(unitOfWork, mapper), ITaxBracketService
    {
        #region Override methods
        public override async Task<GetTaxBracketDTO> CreateAsync(CreateTaxBracketDTO createTaxBracketDTO)
        {
            var taxBracket = _mapper.Map<TaxBracket>(createTaxBracketDTO);

            var result = await _repository.AddAsync(taxBracket);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }

        public override async Task<GetTaxBracketDTO> UpdateAsync(int id, UpdateTaxBracketDTO updateTaxBracketDTO)
        {
            var taxBracket = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(TaxBracket), id);

            _mapper.Map(updateTaxBracketDTO, taxBracket);

            var result = _repository.Update(taxBracket);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(result.Id);
        }
        #endregion

        #region Primary methods
        public async Task<List<GetTaxBracketDTO>> GetCurrentConfigAsync()
        {
            var currentConfig = await _repository.GetAllAsync();
            if (currentConfig.Count == 0)
            {
                throw new DataNotFoundException("No tax bracket found.");
            }

            return _mapper.Map<List<GetTaxBracketDTO>>(currentConfig);
        }
        #endregion
    }
}
