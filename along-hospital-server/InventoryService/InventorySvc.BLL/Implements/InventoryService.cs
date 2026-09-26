using AutoMapper;
using InventorySvc.BLL.DTOs;
using InventorySvc.BLL.Interfaces;
using InventorySvc.DAL.Models;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace InventorySvc.BLL.Implements
{
    public class InventoryService(
        IUnitOfWork _unitOfWork,
        IMessageBus messageBus,
        IMapper _mapper)
            : BaseService<Inventory, CreateInventoryDTO, UpdateInventoryDTO, GetInventoryDTO>(_unitOfWork, _mapper),
                IInventoryService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<int> GetQuantityBySKUCodeAsync(string skuCode)
        {
            var inventory = await _repository.GetByConditionAsync(i => i.SKUCode == skuCode);
            return inventory?.Quantity ?? 0;
        }

        public async Task<GetInventoryDTO> UpdateBySKUCodeAsync(string skuCode, UpdateInventoryDTO updateDTO)
        {
            var inventory = await _repository.GetByConditionAsync(i => i.SKUCode == skuCode)
                ?? throw new DataNotFoundException($"Inventory for SKU ({skuCode}) was not found");

            return await this.UpdateAsync(inventory.Id, updateDTO);
        }

        // No need to use delete contract
        public async Task DeleteBySKUCodeAsync(string skuCode)
        {
            var inventory = await _repository.GetByConditionAsync(i => i.SKUCode == skuCode);
            if (inventory != null)
            {
                _repository.Remove(inventory);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task<GetInventoryDTO> GetInventoryBySKUCodeAsync(string skuCode)
        {
            var inventory = await _repository.GetByConditionAsync(i => i.SKUCode == skuCode)
                ?? throw new DataNotFoundException($"Inventory for SKU ({skuCode}) was not found");
            return _mapper.Map<GetInventoryDTO>(inventory);
        }

        public async Task SendLowStockAlertEmailsAsync()
        {
            var lowStockEntities = await _repository.GetAllAsync(i => i.Quantity <= i.MinQuantity);

            if (lowStockEntities.Count == 0)
            {
                return;
            }

            var lowStockInventories = _mapper.Map<List<GetInventoryDTO>>(lowStockEntities)
                .Where(inv => !string.IsNullOrEmpty(inv.SKUCode))
                .ToList();

            if (lowStockInventories.Count == 0)
            {
                return;
            }

            var skuCodes = lowStockInventories
                .Select(i => i.SKUCode!)
                .Distinct()
                .ToList();

            var medicineContract = await _messageBus.RequestAsync<GetListMedicineDataBySKUCodesEvent, GetListMedicineSKUDataContract>(
                new GetListMedicineDataBySKUCodesEvent { SKUCodes = skuCodes });

            if (medicineContract.Data.Count == 0)
            {
                return;
            }

            var medicineDict = medicineContract.Data
                .Select(c => _mapper.Map<GetMedicineDTO>(c))
                .Where(m => !string.IsNullOrEmpty(m.SKUCode))
                .ToDictionary(m => m.SKUCode!);

            foreach (var inventory in lowStockInventories)
            {
                if (medicineDict.TryGetValue(inventory.SKUCode!, out var med))
                {
                    inventory.Medicine = med;
                }
            }

            var lowStockMedicines = _mapper.Map<List<SendLowStockMedicineEmailEvent>>(
                lowStockInventories.Where(inv => inv.Medicine != null).ToList());

            var emailEvent = new SendLowStockMedicinesEmailEvent { Data = lowStockMedicines };
            await _messageBus.PublishAsync(emailEvent);
        }

        public async Task<List<GetInventoryDTO>> GetInventoriesBySKUCodesAsync(List<string> skuCodes)
        {
            var inventories = await _repository.GetAllAsync(i => skuCodes.Contains(i.SKUCode));
            return _mapper.Map<List<GetInventoryDTO>>(inventories);
        }
    }
}