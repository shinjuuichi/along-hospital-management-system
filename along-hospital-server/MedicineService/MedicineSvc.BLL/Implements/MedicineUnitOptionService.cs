using MedicineSvc.BLL.DTOs.MedicineUnitOptionDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.Exceptions;

namespace MedicineSvc.BLL.Implements
{
    public class MedicineUnitOptionService(
        IUnitOfWork unitOfWork)
        : IMedicineUnitOptionService
    {
        private readonly IGenericRepository<MedicineUnitOption> _repository = unitOfWork.Repository<MedicineUnitOption>();

        public async Task UpdateStatusAsync(UpdateMedicineUnitOptionDTO updateDto)
        {
            var entity = await _repository.GetByConditionAsync(
                x => x.MedicineUnitId == updateDto.MedicineUnitId &&
                x.OptionId == updateDto.OptionId)
                    ?? throw new DataNotFoundException("Medicine Unit Option not found");

            entity.IsActive = updateDto.IsActive;
            _repository.Update(entity);
            await unitOfWork.SaveChangeAsync();
        }
    }
}