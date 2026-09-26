using AutoMapper;
using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.DTOs.MedicineSkuDTOs;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Enums;
using MedicineSvc.DAL.Models;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Services.Interfaces;

namespace MedicineSvc.BLL.Implements
{
    public class MedicineService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadFileService uploadFileService,
        IMessageBus messageBus)
            : BaseService<Medicine, CreateMedicineDTO, UpdateMedicineAndInventoryDTO, GetMedicineDTO>(
                unitOfWork,
                mapper,
                uploadFileService,
                includes:
                [
                    nameof(Medicine.MedicineCategory),
                    $"{nameof(Medicine.Skus)}.{nameof(MedicineSKU.SkuValues)}.{nameof(SKUValue.OptionValue)}",
                    $"{nameof(Medicine.MedicineUnit)}.{nameof(MedicineUnit.MedicineUnitOptions)}" +
                        $".{nameof(MedicineUnitOption.Option)}.{nameof(Option.OptionValues)}",
                ]),
                    IMedicineService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IGenericRepository<MedicineSKU> _skuRepository = unitOfWork.Repository<MedicineSKU>();

        public async Task<bool> CheckExistByIdAsync(int medicineId)
        {
            var exists = await _repository.AnyAsync(x => x.Id == medicineId);
            return exists;
        }

        public async Task<List<GetMedicineDTO>> GetAllByNamesAsync(List<string> names)
        {
            var namesLower = names.Select(n => n.ToLower()).ToList();
            var medicines = await _repository.GetAllAsync(
                m => m.Name != null && namesLower.Contains(m.Name.ToLower()),
                _includes);

            return _mapper.Map<List<GetMedicineDTO>>(medicines);
        }

        public async Task<List<GetMedicineSKUDTO>> GetAllBySKUCodesAsync(List<string> skuCodes)
        {
            var skuCodesLower = skuCodes.Select(c => c.ToLower()).ToList();

            var skus = await _skuRepository.GetAllAsync(
                s => s.SKUCode != null && skuCodesLower.Contains(s.SKUCode.ToLower()),
                includes: [
                    $"{nameof(MedicineSKU.Medicine)}.{nameof(Medicine.MedicineCategory)}",
                    $"{nameof(MedicineSKU.Medicine)}.{nameof(Medicine.MedicineUnit)}",
                    $"{nameof(MedicineSKU.SkuValues)}.{nameof(SKUValue.OptionValue)}.{nameof(OptionValue.Option)}"
                ]);

            if (skus.Count == 0)
            {
                return [];
            }

            return _mapper.Map<List<GetMedicineSKUDTO>>(skus);
        }

        public override async Task<GetMedicineDTO> UpdateAsync(int id, UpdateMedicineAndInventoryDTO updateDTO)
        {
            var hasActiveSku = await _skuRepository.AnyAsync(s => s.MedicineId == id && s.IsActive);
            if (updateDTO.Status == nameof(MedicineStatusEnum.Active) && !hasActiveSku)
            {
                throw new InvalidDataException("Cannot activate the medicine because it has no active SKUs.");
            }

            return await base.UpdateAsync(id, updateDTO);
        }

        public override async Task<GetMedicineDTO> GetByIdAsync(int id)
        {
            var medicine = await base.GetByIdAsync(id);

            await this.GetSkusWithInventoryAndDiscountAsync(medicine.SKUs, medicine.Id);

            return medicine;
        }

        public override async Task<PaginationResult<GetMedicineDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var getAll = await base.GetAllPaginatedAsync(filterDTO);

            await this.GetSkusWithInventoryAndDiscountAsync(getAll);

            return getAll;
        }

        public async Task<List<GetMedicineDTO>> GetAllInfusionMedicinesAsync()
        {
            var medicines = await _repository.GetAllAsync(
                m => m.MedicineCategory != null && m.MedicineCategory.Name == MedicineCategoryNameConstants.INFUSION_SOLUTIONS_NAME,
                _includes);

            return _mapper.Map<List<GetMedicineDTO>>(medicines);
        }

        public async Task<List<CreatedMedicineFromExcelDTO>> CreateMedicinesFromExcelAsync(List<CreateMedicineFromExcelEventItem> medicines)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var createdMedicineDTOs = new List<CreatedMedicineFromExcelDTO>();

                foreach (var medicineItem in medicines)
                {
                    var createMedicineDto = _mapper.Map<CreateMedicineFromExcelDTO>(medicineItem);
                    var medicine = _mapper.Map<Medicine>(createMedicineDto);

                    var createdMedicine = await _repository.AddAsync(medicine);
                    await _unitOfWork.SaveChangeAsync();

                    var createdMedicineDTO = _mapper.Map<CreatedMedicineFromExcelDTO>(createdMedicine);
                    createdMedicineDTOs.Add(createdMedicineDTO);
                }

                await _unitOfWork.CommitTransactionAsync();

                return createdMedicineDTOs;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task GetSkusWithInventoryAndDiscountAsync(IEnumerable<GetMedicineSKUDTO> skus, int medicineId)
        {
            var skuCodes = skus
                .Select(sku => sku.SKUCode)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .OfType<string>()
                .Distinct()
                .ToList();

            if (skuCodes.Count == 0)
            {
                return;
            }

            var inventoryContract = await _messageBus
                .RequestAsync<GetListInventoryDataBySKUCodesEvent, GetListInventoryDataBySKUCodesContract>(
                    new() { SKUCodes = skuCodes });

            var previewList = _mapper.Map<List<PreviewMedicineDetailEvent>>(skus);

            var discountContract = await _messageBus
                .RequestAsync<PreviewListMedicineDiscountEvent, PreviewListMedicineDiscountContract>(
                    new() { Medicines = previewList });

            var discountDict = discountContract.MedicineDiscounts
                .Where(d => !string.IsNullOrWhiteSpace(d.SKUCode))
                .ToDictionary(d => d.SKUCode!, d => d);

            var inventoryDict = inventoryContract.Data
                .Where(i => !string.IsNullOrWhiteSpace(i.SKUCode))
                .ToDictionary(i => i.SKUCode!, i => i);

            foreach (var sku in skus)
            {
                if (sku.SKUCode != null && discountDict.TryGetValue(sku.SKUCode, out var discount))
                {
                    sku.OrigionPrice = sku.Price;
                    sku.Price = discount.FinalPrice;
                }

                if (sku.SKUCode != null && inventoryDict.TryGetValue(sku.SKUCode, out var inventory))
                {
                    sku.Inventory = _mapper.Map<GetInventoryDTO>(inventory);
                }
            }
        }

        private async Task GetSkusWithInventoryAndDiscountAsync(PaginationResult<GetMedicineDTO> paginatedResult)
        {
            var allSkus = paginatedResult.Collection
                .SelectMany(medicine => medicine.SKUs)
                .ToList();

            var skuCodes = allSkus
                .Select(sku => sku.SKUCode)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .OfType<string>()
                .Distinct()
                .ToList();

            if (skuCodes.Count == 0)
            {
                return;
            }

            var inventoryContract = await _messageBus
                .RequestAsync<GetListInventoryDataBySKUCodesEvent, GetListInventoryDataBySKUCodesContract>(
                    new() { SKUCodes = skuCodes });

            var previewList = paginatedResult.Collection
                .SelectMany(medicine => medicine.SKUs
                    .Select(sku => new PreviewMedicineDetailEvent
                    {
                        MedicineId = medicine.Id,
                        SKUCode = sku.SKUCode,
                        Price = sku.Price
                    }))
                .ToList();

            var discountContract = await _messageBus
                .RequestAsync<PreviewListMedicineDiscountEvent, PreviewListMedicineDiscountContract>(
                    new() { Medicines = previewList });

            var discountDict = discountContract.MedicineDiscounts
                .Where(d => !string.IsNullOrWhiteSpace(d.SKUCode))
                .ToDictionary(d => d.SKUCode!, d => d);

            var inventoryDict = inventoryContract.Data
                .Where(i => !string.IsNullOrWhiteSpace(i.SKUCode))
                .ToDictionary(i => i.SKUCode!, i => i);

            foreach (var medicine in paginatedResult.Collection)
            {
                foreach (var sku in medicine.SKUs)
                {
                    if (sku.SKUCode != null && discountDict.TryGetValue(sku.SKUCode, out var discount))
                    {
                        sku.OrigionPrice = sku.Price;
                        sku.Price = discount.FinalPrice;
                    }

                    if (sku.SKUCode != null && inventoryDict.TryGetValue(sku.SKUCode, out var inventory))
                    {
                        sku.Inventory = _mapper.Map<GetInventoryDTO>(inventory);
                    }
                }
            }
        }
    }
}
