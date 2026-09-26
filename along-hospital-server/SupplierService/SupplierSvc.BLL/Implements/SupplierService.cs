using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SupplierSvc.BLL.DTOs.SupplierDTOs;
using SupplierSvc.BLL.Interfaces;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.BLL.Implements
{
    public class SupplierService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper)
      : BaseService<Supplier, UpsertSupplierDTO, UpsertSupplierDTO, GetSupplierDTO>(_unitOfWork, _mapper),
            ISupplierService
    {
        public async Task<GetSupplierDTO> GetByNameAsync(string? supplierName)
        {
            var supplier = await _repository.GetByConditionAsync(s => s.Name == supplierName)
                ?? throw new DataNotFoundException(
                    $"Supplier with name '{supplierName}' not found. " +
                    "Please select an existing supplier from the database.");
            return _mapper.Map<GetSupplierDTO>(supplier);
        }

        public async Task<GetSupplierDTO> GetOrCreateByNameAsync(string supplierName)
        {
            var supplierNameLower = supplierName.ToLower();
            var existingSupplier = await _repository.GetByConditionAsync(
                s => s.Name.ToLower() == supplierNameLower);

            if (existingSupplier != null)
            {
                return _mapper.Map<GetSupplierDTO>(existingSupplier);
            }

            var supplier = new Supplier
            {
                Name = supplierName,
            };
            var createdSupplier = await _repository.AddAsync(supplier);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<GetSupplierDTO>(createdSupplier);
        }
    }
}
