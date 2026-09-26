using AutoMapper;
using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Enums;
using MedicineSvc.DAL.Models;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Events.InventoryEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace MedicineSvc.BLL.Implements
{
    public class MedicineSKUService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : BaseService<MedicineSKU, CreateMedicineSKUDTO, UpdateMedicineSKUDTO, GetMedicineSKUDTO>(
            unitOfWork,
            mapper,
            includes: [
                $"{nameof(MedicineSKU.Medicine)}.{nameof(Medicine.MedicineCategory)}",
                $"{nameof(MedicineSKU.SkuValues)}.{nameof(SKUValue.OptionValue)}",
                $"{nameof(MedicineSKU.Medicine)}.{nameof(Medicine.MedicineUnit)}" +
                    $".{nameof(MedicineUnit.MedicineUnitOptions)}.{nameof(MedicineUnitOption.Option)}.{nameof(Option.OptionValues)}",

            ]),
            IMedicineSKUService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IGenericRepository<MedicineSKU> _skuRepository = unitOfWork.Repository<MedicineSKU>();
        private readonly IGenericRepository<Medicine> _medicineRepository = unitOfWork.Repository<Medicine>();
        private readonly IGenericRepository<OptionValue> _optionValueRepository = unitOfWork.Repository<OptionValue>();
        private readonly IGenericRepository<MedicineUnitOption> _medicineUnitOptionRepository = unitOfWork.Repository<MedicineUnitOption>();
        private readonly IGenericRepository<SKUValue> _skuValuesRepository = unitOfWork.Repository<SKUValue>();

        #region Check Methods
        public async Task<bool> CheckExistByIdAsync(int medicineSkuId)
        {
            return await _skuRepository.AnyAsync(s => s.Id == medicineSkuId);
        }

        public async Task<bool> CheckExistBySKUCodeAsync(string skuCode)
        {
            if (string.IsNullOrWhiteSpace(skuCode))
            {
                return false;
            }

            return await _skuRepository.AnyAsync(s => s.SKUCode == skuCode);
        }
        #endregion

        #region Get Methods
        public async Task<List<GetMedicineSKUDTO>> GetAllBySKUCodesAsync(List<string> skuCodes)
        {
            var distinctSkuCodes = skuCodes
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToList();

            if (distinctSkuCodes.Count == 0)
            {
                throw new DataNotFoundException("SKU codes must contain at least one non-empty SKU code.");
            }

            var skus = await _skuRepository.GetAllAsync(s => distinctSkuCodes.Contains(s.SKUCode), _includes);

            var skuDtos = _mapper.Map<List<GetMedicineSKUDTO>>(skus);
            await this.MapInventoryAsync(skuDtos, distinctSkuCodes);

            return skuDtos;
        }

        public async Task<List<GetMedicineSKUDTO>> GetAllPublicBySKUCodesAsync(List<string> skuCodes)
        {
            var distinctSkuCodes = skuCodes
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToList();

            if (distinctSkuCodes.Count == 0)
            {
                return [];
            }

            var skus = await _skuRepository.GetAllAsync(
                s => distinctSkuCodes.Contains(s.SKUCode) && s.Medicine!.IsPublic,
                _includes);

            var skuDtos = _mapper.Map<List<GetMedicineSKUDTO>>(skus);
            await this.MapInventoryAsync(skuDtos, distinctSkuCodes);
            return skuDtos;
        }

        public override async Task<PaginationResult<GetMedicineSKUDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO);

            var skuCodes = result.Collection
                .Where(s => !string.IsNullOrWhiteSpace(s.SKUCode))
                .Select(s => s.SKUCode!)
                .Distinct()
                .ToList();

            if (skuCodes.Count == 0)
            {
                return result;
            }

            await this.MapInventoryAsync(result.Collection.ToList(), skuCodes);

            return result;
        }
        #endregion

        #region Create/Update Methods
        public override async Task<GetMedicineSKUDTO> CreateAsync(CreateMedicineSKUDTO createDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Ensure medicine exists
                var medicine = await _medicineRepository.GetByIdAsync(createDto.MedicineId, [
                    nameof(Medicine.MedicineUnit),
                    $"{nameof(Medicine.MedicineUnit)}.{nameof(MedicineUnit.MedicineUnitOptions)}"
                ]);
                if (medicine == null)
                {
                    throw new DataNotFoundException($"Medicine with id {createDto.MedicineId} not found");
                }

                // Remove duplicated option values
                var createOptionValueIds = createDto.OptionValueIds.Distinct().ToList();

                if (createOptionValueIds.Count == 0)
                {
                    throw new InvalidDataException("SKU must have at least 1 option value");
                }

                var optionValues = (await _optionValueRepository
                    .GetAllAsync(ov => createOptionValueIds.Contains(ov.Id)))
                    .ToList();

                // All option values must exist
                if (optionValues.Count != createOptionValueIds.Count)
                {
                    throw new DataNotFoundException("One or more option values not found");
                }

                // Each option must have only one selected value
                var optionIdsFromValues = optionValues.Select(ov => ov.OptionId).ToList();

                if (optionIdsFromValues.GroupBy(x => x).Any(g => g.Count() > 1))
                {
                    throw new DataNotFoundException("Each option must have exactly one option value for a SKU");
                }

                // Option values must belong to active options of the medicine unit
                var medicineUnitOptions = await _medicineUnitOptionRepository
                    .GetAllAsync(muo =>
                        muo.MedicineUnitId == medicine.MedicineUnitId &&
                        muo.IsActive
                    );

                var validOptionIds = medicineUnitOptions
                    .Select(muo => muo.OptionId)
                    .ToHashSet();

                if (optionIdsFromValues.Any(oid => !validOptionIds.Contains(oid)))
                {
                    throw new DataNotFoundException("One or more option values are not valid for this medicine unit");
                }

                // Generate SKU code based on medicine and option values
                var skuCode = this.GenerateSKUCode(
                    medicine.Name,
                    medicine.Id,
                    optionValues.Select(ov => ov.ValueName).OrderBy(vn => vn).ToList()
                );

                var newSku = new MedicineSKU
                {
                    SKUCode = skuCode,
                    Name = createDto.Name ?? string.Empty,
                    Price = createDto.Price,
                    MedicineId = createDto.MedicineId
                };

                var createSku = await _skuRepository.AddAsync(newSku);
                await _unitOfWork.SaveChangeAsync();

                // Activate medicine when at least one SKU exists
                if (medicine.Status == MedicineStatusEnum.Draft)
                {
                    medicine.Status = MedicineStatusEnum.Active;
                    _medicineRepository.Update(medicine);
                }

                // Bind option values to SKU
                var skuValues = createOptionValueIds.Select(ovId => new SKUValue
                {
                    MedicineSKUId = createSku.Id,
                    OptionValueId = ovId
                }).ToList();

                await _skuValuesRepository.AddRangeAsync(skuValues);
                await _unitOfWork.SaveChangeAsync();

                // Create inventory for the new SKU
                await _messageBus.RequestAsync<CreateInventoryEvent, CreateInventoryContract>(new()
                {
                    SKUCode = createSku.SKUCode,
                    Quantity = createDto.Quantity,
                    MinQuantity = createDto.MinQuantity,
                    MaxQuantity = createDto.MaxQuantity
                });

                await _unitOfWork.CommitTransactionAsync();
                return await this.GetByIdAsync(newSku.Id);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public override async Task<GetMedicineSKUDTO> UpdateAsync(int id, UpdateMedicineSKUDTO updateDto)
        {
            try
            {
                var existingSku = await _skuRepository.GetByIdAsync(id);
                if (existingSku == null)
                {
                    throw new DataNotFoundException($"MedicineSku with id {id} not found");
                }

                var currentInventory = await _messageBus.RequestAsync<GetInventoryBySKUCodeEvent, GetInventoryBySKUCodeContract>(new()
                {
                    SKUCode = existingSku.SKUCode
                });

                await _unitOfWork.BeginTransactionAsync();

                if (!updateDto.IsActive)
                {
                    var hasActiveSku = await _skuRepository.AnyAsync(s => s.MedicineId == existingSku.MedicineId && s.IsActive && s.Id != id);
                    if (!hasActiveSku)
                    {
                        var medicine = await _medicineRepository.GetByIdAsync(existingSku.MedicineId);
                        if (medicine != null && medicine.Status == MedicineStatusEnum.Active)
                        {
                            medicine.Status = MedicineStatusEnum.Inactive;
                            _medicineRepository.Update(medicine);
                        }
                    }
                }

                await _messageBus.RequestAsync<UpdateInventoryEvent, UpdateInventoryContract>(new()
                {
                    SKUCode = existingSku.SKUCode,
                    Quantity = currentInventory.Quantity,
                    LastImportDate = currentInventory.LastImportDate,
                    MinQuantity = updateDto.MinQuantity,
                    MaxQuantity = updateDto.MaxQuantity
                });

                var updated = await base.UpdateAsync(id, updateDto);
                await _unitOfWork.CommitTransactionAsync();
                return updated;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        #endregion

        #region Private Methods
        private string GenerateSKUCode(string medicineName, int medicineId, List<string> valueNames)
        {
            var medicinePrefix = medicineName.Length >= 2
                ? medicineName.Substring(0, 2).ToUpper()
                : medicineName.ToUpper();

            var valuePrefixes = string.Join("", valueNames.Select(vn =>
                vn.Length >= 2 ? vn.Substring(0, 2).ToUpper() : vn.ToUpper()
            ));

            return $"{medicinePrefix}{medicineId}{valuePrefixes}";
        }

        private async Task MapInventoryAsync(List<GetMedicineSKUDTO> skus, List<string> skuCodes)
        {
            if (skuCodes.Count == 0)
            {
                return;
            }

            var getListInventoryDataBySKUCodesContract = await _messageBus.RequestAsync<
                    GetListInventoryDataBySKUCodesEvent, GetListInventoryDataBySKUCodesContract>(new()
                    {
                        SKUCodes = skuCodes
                    });
            var inventoryData = _mapper.Map<List<GetInventoryDTO>>(getListInventoryDataBySKUCodesContract.Data);
            var inventoryDict = inventoryData
                .Where(i => i.SKUCode != null)
                .ToDictionary(i => i.SKUCode!, i => i);

            foreach (var sku in skus)
            {
                if (sku.SKUCode != null && inventoryDict.TryGetValue(sku.SKUCode, out var inventory))
                {
                    sku.Inventory = inventory;
                }
            }
        }
        #endregion
    }
}
