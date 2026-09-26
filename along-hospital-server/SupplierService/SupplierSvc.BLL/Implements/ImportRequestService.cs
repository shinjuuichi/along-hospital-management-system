using AutoMapper;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;
using SupplierSvc.BLL.DTOs.ImportRequestDTOs;
using SupplierSvc.BLL.Interfaces;
using SupplierSvc.BLL.StateMachines;
using SupplierSvc.DAL.Enums;
using SupplierSvc.DAL.Models;
using SupplierSvc.DAL.Models.Snapshots;

namespace SupplierSvc.BLL.Implements
{
    public class ImportRequestService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService
        )
        : BaseService<ImportRequest, CreateImportRequestDTO, UpdateImportRequestDTO, GetImportRequestDTO>(
            unitOfWork, mapper, includes: [nameof(ImportRequest.ImportRequestDetails)]),
          IImportRequestService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private const string DetailsEmptyMessage = "Details cannot be empty. Please provide at least one detail.";

        public override async Task<GetImportRequestDTO> CreateAsync(CreateImportRequestDTO createDTO)
        {
            if (createDTO.Details.Count == 0)
            {
                throw new InvalidDataException(DetailsEmptyMessage);
            }

            if (createDTO.Details.Any(d => string.IsNullOrWhiteSpace(d.SKUCode)))
            {
                throw new InvalidDataException("Each detail must have a non-empty SKUCode.");
            }

            var skuCodes = createDTO.Details.Select(d => d.SKUCode!).ToList();
            var duplicateSkus = skuCodes.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            if (duplicateSkus.Count > 0)
            {
                throw new InvalidDataException($"Duplicate SKUCode detected: [{string.Join(", ", duplicateSkus)}]");
            }

            var distinctSkuCodes = skuCodes.Distinct().ToList();

            var skuToName = await this.GetSkuToMedicineNameDictAsync(distinctSkuCodes);
            await this.ValidateInventoryCapacityAsync(createDTO.Details
                .Select(d => (d.SKUCode!, d.RequestQuantity))
                .ToList());

            var entity = _mapper.Map<ImportRequest>(createDTO);
            entity.CreatedBy = _currentUserService.UserId;

            foreach (var detail in entity.ImportRequestDetails)
            {
                detail.MedicineSnapshot = new ImportRequestDetailSnapshot
                {
                    MedicineName = skuToName[detail.SKUCode]
                };
            }

            var resultEntity = await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(resultEntity.Id);
        }

        public override async Task<GetImportRequestDTO> UpdateAsync(int id, UpdateImportRequestDTO updateDTO)
        {
            var importRequest = await _repository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(ImportRequest), id);

            if (importRequest.CreatedBy != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException(
                    $"You can only update your own ImportRequest. ImportRequest #{id} was created by another user.");
            }

            if (importRequest.Status != ImportRequestStatusEnum.Pending)
            {
                throw new InvalidDataException(
                  $"Can only update ImportRequest in Pending status. Current status: {importRequest.Status}");
            }

            var skuCodes = updateDTO.Details
                .Where(d => !string.IsNullOrEmpty(d.SKUCode))
                .Select(d => d.SKUCode!)
                .ToList();

            var duplicateSkus = skuCodes.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();

            if (duplicateSkus.Count > 0)
            {
                throw new InvalidDataException($"Duplicate SKUCode detected: [{string.Join(", ", duplicateSkus)}]");
            }

            var distinctSkuCodes = skuCodes.Distinct().ToList();

            var skuToName = await this.GetSkuToMedicineNameDictAsync(distinctSkuCodes);
            await this.ValidateInventoryCapacityAsync(updateDTO.Details
                .Where(d => !string.IsNullOrEmpty(d.SKUCode))
                .Select(d => (d.SKUCode!, d.RequestQuantity))
                .ToList());

            var detailDict = importRequest.ImportRequestDetails
                .Where(d => !string.IsNullOrEmpty(d.SKUCode))
                .ToDictionary(d => d.SKUCode!, StringComparer.OrdinalIgnoreCase);

            foreach (var updateDetail in updateDTO.Details)
            {
                if (string.IsNullOrEmpty(updateDetail.SKUCode))
                {
                    throw new InvalidDataException("SKUCode is required for each update detail.");
                }

                if (!detailDict.TryGetValue(updateDetail.SKUCode, out var detail))
                {
                    var newDetail = _mapper.Map<ImportRequestDetail>(updateDetail);
                    newDetail.ImportRequestId = id;
                    newDetail.MedicineSnapshot = new ImportRequestDetailSnapshot
                    {
                        MedicineName = skuToName[updateDetail.SKUCode]
                    };
                    importRequest.ImportRequestDetails.Add(newDetail);
                    continue;
                }

                _mapper.Map(updateDetail, detail);
            }

            _repository.Update(importRequest);
            await _unitOfWork.SaveChangeAsync();
            return await this.GetByIdAsync(id);
        }

        public async Task ChangeStatusAsync(int id, ImportRequestStatusEnum targetStatus)
        {
            var importRequest = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(ImportRequest), id);

            this.ChangeStatus(importRequest, targetStatus);

            await _unitOfWork.SaveChangeAsync();
        }

        private void ChangeStatus(ImportRequest importRequest, ImportRequestStatusEnum targetStatus)
        {
            var stateMachine = new ImportRequestStatusStateMachine(importRequest, _currentUserService.UserId);

            if (!stateMachine.CanFire(targetStatus))
            {
                throw new InvalidDataException(
                  $"Cannot change ImportRequest status from {importRequest.Status} to {targetStatus}");
            }

            stateMachine.Fire(targetStatus);

            _repository.Update(importRequest);
        }

        private async Task<Dictionary<string, string>> GetSkuToMedicineNameDictAsync(List<string> skuCodes)
        {
            var getListMedicineSkuDataByCodesEvent = new GetListMedicineDataBySKUCodesEvent { SKUCodes = skuCodes };
            var contract = await _messageBus
                .RequestAsync<GetListMedicineDataBySKUCodesEvent, GetListMedicineSKUDataContract>(
                    getListMedicineSkuDataByCodesEvent);

            var skuDict = contract.Data
                .Where(s => !string.IsNullOrEmpty(s.SKUCode))
                .ToDictionary(s => s.SKUCode!, s => s.MedicineName ?? "");

            if (skuDict.Count != skuCodes.Count)
            {
                var notFoundCodes = skuCodes.Where(code => !skuDict.ContainsKey(code)).ToList();
                throw new DataNotFoundException($"SKUCodes [{string.Join(", ", notFoundCodes)}] not found");
            }

            return skuDict;
        }

        private async Task ValidateInventoryCapacityAsync(List<(string SKUCode, int Quantity)> details)
        {
            var skuCodes = details.Select(d => d.SKUCode).ToList();
            var getInventoryEvent = new GetListInventoryDataBySKUCodesEvent { SKUCodes = skuCodes };
            var inventoryContract = await _messageBus
                .RequestAsync<GetListInventoryDataBySKUCodesEvent, GetListInventoryDataBySKUCodesContract>(getInventoryEvent);

            var inventoryDict = inventoryContract.Data
                .Where(i => !string.IsNullOrEmpty(i.SKUCode))
                .ToDictionary(i => i.SKUCode!);

            foreach (var (skuCode, requestQuantity) in details)
            {
                if (!inventoryDict.TryGetValue(skuCode, out var inventory))
                {
                    continue;
                }

                if (inventory.MaxQuantity.HasValue)
                {
                    var newQuantity = inventory.Quantity + requestQuantity;
                    if (newQuantity > inventory.MaxQuantity.Value)
                    {
                        throw new InvalidDataException(
                            $"Quantity after applying this import request ({newQuantity}) exceeds MaxQuantity ({inventory.MaxQuantity.Value}) " +
                            $"for SKUCode {skuCode}");
                    }
                }
            }
        }
    }
}