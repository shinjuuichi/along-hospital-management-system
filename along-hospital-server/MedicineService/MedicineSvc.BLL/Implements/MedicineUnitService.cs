using AutoMapper;
using MedicineSvc.BLL.DTOs.MedicineUnitDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace MedicineSvc.BLL.Implements
{
    public class MedicineUnitService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<MedicineUnit, CreateMedicineUnitDTO, UpdateMedicineUnitDTO, GetMedicineUnitDTO>(
            unitOfWork,
            mapper,
            includes: [$"{nameof(MedicineUnit.MedicineUnitOptions)}.{nameof(MedicineUnitOption.Option)}"]),
            IMedicineUnitService
    {
        private readonly IGenericRepository<Option> _optionRepository = unitOfWork.Repository<Option>();

        private readonly IGenericRepository<MedicineUnitOption> _medicineUnitOptionRepository = unitOfWork.Repository<MedicineUnitOption>();

        public override async Task<GetMedicineUnitDTO> CreateAsync(CreateMedicineUnitDTO createDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var requestedOptionIds = createDto.OptionIds
                    .Distinct()
                    .ToList();

                var existingOptions = await _optionRepository.GetAllAsync(
                    option => requestedOptionIds.Contains(option.Id)
                );

                if (existingOptions.Count != requestedOptionIds.Count)
                {
                    throw new DataNotFoundException("One or more requested options were not found");
                }

                var createdMedicineUnit = await base.CreateAsync(createDto);

                var medicineUnitOptions = requestedOptionIds.Select(optionId => new MedicineUnitOption
                {
                    MedicineUnitId = createdMedicineUnit.Id,
                    OptionId = optionId,
                    IsActive = true
                }).ToList();

                await _medicineUnitOptionRepository.AddRangeAsync(medicineUnitOptions);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitTransactionAsync();

                return createdMedicineUnit;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public override async Task<GetMedicineUnitDTO> UpdateAsync(int id, UpdateMedicineUnitDTO updateDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (updateDto.OptionIds != null)
                {
                    var requestedOptionIds = updateDto.OptionIds
                        .Distinct()
                        .ToList();

                    var existingOptions = await _optionRepository.GetAllAsync(
                        option => requestedOptionIds.Contains(option.Id));

                    if (existingOptions.Count != requestedOptionIds.Count)
                    {
                        var existingOptionIds = existingOptions
                            .Select(o => o.Id)
                            .ToHashSet();

                        var missingOptionIds = requestedOptionIds
                            .Where(id => !existingOptionIds.Contains(id))
                            .ToList();

                        if (existingOptions.Count != requestedOptionIds.Count)
                        {
                            throw new DataNotFoundException("One or more requested options were not found");
                        }
                    }

                    var currentMedicineUnitOptions = await _medicineUnitOptionRepository.GetAllAsync(
                        muo => muo.MedicineUnitId == id
                    );

                    var currentOptionIds = currentMedicineUnitOptions
                        .Select(muo => muo.OptionId)
                        .ToHashSet();

                    var optionsToRemove = currentMedicineUnitOptions
                        .Where(muo => !requestedOptionIds.Contains(muo.OptionId))
                        .ToList();

                    var optionsToAdd = requestedOptionIds
                        .Where(optionId => !currentOptionIds.Contains(optionId))
                        .Select(optionId => new MedicineUnitOption
                        {
                            MedicineUnitId = id,
                            OptionId = optionId,
                            IsActive = true
                        })
                        .ToList();

                    if (optionsToRemove.Count > 0)
                    {
                        _medicineUnitOptionRepository.RemoveRange(optionsToRemove);
                    }

                    if (optionsToAdd.Count > 0)
                    {
                        await _medicineUnitOptionRepository.AddRangeAsync(optionsToAdd);
                    }
                }

                var updatedDTO = await base.UpdateAsync(id, updateDto);

                await _unitOfWork.CommitTransactionAsync();

                return updatedDTO;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}