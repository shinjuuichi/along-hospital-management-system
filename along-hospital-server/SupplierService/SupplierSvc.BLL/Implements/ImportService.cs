using AutoMapper;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using SupplierSvc.BLL.DTOs.ImportDTOs;
using SupplierSvc.BLL.Interfaces;
using SupplierSvc.DAL.Enums;
using SupplierSvc.DAL.Models;
using SupplierSvc.DAL.Models.Snapshots;

namespace SupplierSvc.BLL.Implements
{
    public class ImportService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService,
        ISupplierService supplierService,
        IImportRequestService importRequestService)
        : BaseService<Import, CreateImportDTO, UpdateImportDTO, GetImportDTO>(
            unitOfWork, mapper, includes: [nameof(Import.ImportDetails)]),
          IImportService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ISupplierService _supplierService = supplierService;
        private readonly IImportRequestService _importRequestService = importRequestService;
        private readonly IGenericRepository<ImportRequest> _importRequestRepository = unitOfWork.Repository<ImportRequest>();

        public override async Task<GetImportDTO> GetByIdAsync(int id)
        {
            var result = await base.GetByIdAsync(id);
            await this.RequestValueSupplierNamesAsync([result]);
            return result;
        }

        public override async Task<List<GetImportDTO>> GetAllAsync()
        {
            var results = await base.GetAllAsync();
            await this.RequestValueSupplierNamesAsync(results);
            return results;
        }

        public override async Task<PaginationResult<GetImportDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO);
            await this.RequestValueSupplierNamesAsync(result.Collection);
            return result;
        }

        public async Task<GetImportDTO> CreateImportFromApprovedAsync(CreateImportDTO createDTO)
        {
            var existingImport = await _repository.GetByConditionAsync(i => i.ImportRequestId == createDTO.ImportRequestId);
            if (existingImport != null)
            {
                throw new InvalidDataException(
                  $"An Import already exists for ImportRequest #{createDTO.ImportRequestId}. Cannot create duplicate.");
            }

            var priceOverrides = createDTO.Details
                .Where(d => !string.IsNullOrEmpty(d.SKUCode))
                .GroupBy(d => d.SKUCode!, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().UnitPrice, StringComparer.OrdinalIgnoreCase);

            var import = await this.PrepareImportFromApprovedRequestAsync(
              createDTO.ImportRequestId,
              createDTO.SupplierId,
              createDTO.Note,
              priceOverrides);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _repository.AddAsync(import);
                await _unitOfWork.SaveChangeAsync();
                await this.UpdateInventoriesFromImportAsync(import);
                await this.UpdateUnitPriceSnapshotsAsync(import);
                await _importRequestService.ChangeStatusAsync(
                    import.ImportRequestId, ImportRequestStatusEnum.Created);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return await this.GetByIdAsync(import.Id);
        }

        private async Task<Import> PrepareImportFromApprovedRequestAsync(
          int importRequestId,
          int supplierId,
          string? note,
          Dictionary<string, double>? priceOverrides = null)
        {
            var importRequest = await _importRequestRepository.GetByIdAsync(
              importRequestId, includes: [nameof(ImportRequest.ImportRequestDetails)]);

            if (importRequest == null)
            {
                throw new DataNotFoundException($"ImportRequest with Id {importRequestId} not found");
            }

            if (importRequest.Status != ImportRequestStatusEnum.Approved)
            {
                throw new InvalidDataException(
                  $"ImportRequest must be in Approved status to create Import. Current status: {importRequest.Status}");
            }

            var skuCodes = importRequest.ImportRequestDetails
              .Select(d => d.SKUCode).Distinct().ToList();
            var skuDict = await this.GetSkuDataDictionaryAsync(skuCodes);

            var import = _mapper.Map<Import>(importRequest);
            import.ManagerId = _currentUserService.UserId;
            import.SupplierId = supplierId;
            import.Note = note;

            import.ImportDetails = importRequest.ImportRequestDetails.Select(detail =>
            {
                var importDetail = _mapper.Map<ImportDetail>(detail);
                if (priceOverrides != null && !string.IsNullOrEmpty(detail.SKUCode) && priceOverrides.TryGetValue(detail.SKUCode, out var overridePrice))
                {
                    importDetail.UnitPrice = overridePrice;
                }
                else if (!string.IsNullOrEmpty(detail.SKUCode) && skuDict.TryGetValue(detail.SKUCode, out var skuData))
                {
                    importDetail.UnitPrice = skuData.Price;
                }
                else
                {
                    throw new InvalidDataException($"Cannot determine UnitPrice for SKU {detail.SKUCode}. No price override provided and SKU data not found in catalog.");
                }

                importDetail.Import = import;
                return importDetail;
            }).ToList();

            return import;
        }

        private async Task<Dictionary<string, GetMedicineSKUContract>> GetSkuDataDictionaryAsync(List<string> skuCodes)
        {
            var getListSkuDataEvent = new GetListMedicineDataBySKUCodesEvent { SKUCodes = skuCodes };
            var getListSkuDataContract = await _messageBus
              .RequestAsync<GetListMedicineDataBySKUCodesEvent, GetListMedicineSKUDataContract>(
                getListSkuDataEvent);

            return getListSkuDataContract.Data.ToDictionary(s => s.SKUCode!, StringComparer.OrdinalIgnoreCase);
        }

        private async Task RequestValueSupplierNamesAsync(List<GetImportDTO> importDTOs)
        {
            var allSupplierIds = importDTOs
                .Select(dto => dto.SupplierId)
                .Distinct()
                .ToList();

            if (allSupplierIds.Count == 0)
            {
                return;
            }

            var supplierDTOs = await _supplierService.GetAllByIdsAsync(allSupplierIds);
            var supplierDict = supplierDTOs.ToDictionary(s => s.Id, s => s.Name);

            foreach (var importDTO in importDTOs)
            {
                if (supplierDict.TryGetValue(importDTO.SupplierId, out var supplierName))
                {
                    importDTO.SupplierName = supplierName;
                }
            }
        }

        public async Task UpdateInventoriesFromImportAsync(Import import)
        {
            foreach (var detail in import.ImportDetails)
            {
                var getInventoryEvent = new GetInventoryBySKUCodeEvent { SKUCode = detail.SKUCode };
                GetInventoryBySKUCodeContract? inventoryContract;
                bool isNewInventory = false;

                try
                {
                    inventoryContract = await _messageBus
                        .RequestAsync<GetInventoryBySKUCodeEvent, GetInventoryBySKUCodeContract>(getInventoryEvent);
                }
                catch (DataNotFoundException)
                {
                    var createInventoryEvent = _mapper.Map<CreateInventoryEvent>(detail);
                    createInventoryEvent = createInventoryEvent with { LastImportDate = import.ImportDate };
                    await _messageBus.RequestAsync<CreateInventoryEvent, CreateInventoryContract>(createInventoryEvent);

                    inventoryContract = await _messageBus
                        .RequestAsync<GetInventoryBySKUCodeEvent, GetInventoryBySKUCodeContract>(getInventoryEvent);
                    isNewInventory = true;
                }

                var newQuantity = isNewInventory ? detail.Quantity : inventoryContract!.Quantity + detail.Quantity;

                var addQuantityEvent = _mapper.Map<AddQuantityFromImportEvent>(detail);
                addQuantityEvent = addQuantityEvent with { Quantity = newQuantity, LastImportDate = import.ImportDate };

                await _messageBus
                    .RequestAsync<AddQuantityFromImportEvent, AddQuantityFromImportContract>(addQuantityEvent);
            }
        }

        private async Task UpdateUnitPriceSnapshotsAsync(Import import)
        {
            var importRequest = await _importRequestRepository.GetByIdAsync(
                import.ImportRequestId,
                includes: [nameof(ImportRequest.ImportRequestDetails)]);

            if (importRequest == null)
            {
                return;
            }

            var priceDict = import.ImportDetails
                .ToDictionary(d => d.SKUCode, d => d.UnitPrice);

            foreach (var detail in importRequest.ImportRequestDetails)
            {
                if (!string.IsNullOrEmpty(detail.SKUCode)
                    && priceDict.TryGetValue(detail.SKUCode, out var unitPrice))
                {
                    if (detail.MedicineSnapshot == null)
                    {
                        detail.MedicineSnapshot = new ImportRequestDetailSnapshot();
                    }
                    if (detail.MedicineSnapshot.UnitPrice == null)
                    {
                        detail.MedicineSnapshot.UnitPrice = unitPrice;
                    }
                }
            }

            _importRequestRepository.Update(importRequest);
            await _unitOfWork.SaveChangeAsync();
        }

        //public async Task<BulkImportFromExcelResponseDTO> BulkImportFromExcelAsync(
        //    List<ReadImportRowFromExcelDTO> excelRows)
        //{
        //    await _unitOfWork.BeginTransactionAsync();

        //    try
        //    {
        //        var response = new BulkImportFromExcelResponseDTO
        //        {
        //            TotalRowsProcessed = excelRows.Count
        //        };

        //        var suppliersCreated = 0;
        //        var medicinesCreated = 0;

        //        var processedSuppliers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        //        var medicineIdCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        //        var supplierCache = new Dictionary<string, (int Id, string? Name)>(StringComparer.OrdinalIgnoreCase);

        //        var validRows = new List<ReadImportRowFromExcelDTO>();
        //        ReadImportRowFromExcelDTO? firstValidRow = null;

        //        foreach (var row in excelRows)
        //        {
        //            if (string.IsNullOrWhiteSpace(row.SupplierName) ||
        //                string.IsNullOrWhiteSpace(row.MedicineName) ||
        //                row.Quantity <= 0)
        //            {
        //                continue;
        //            }

        //            firstValidRow ??= row;

        //            var supplierName = row.SupplierName!;
        //            if (!supplierCache.TryGetValue(supplierName, out var supplierInfo))
        //            {
        //                if (!processedSuppliers.Contains(supplierName))
        //                {
        //                    try
        //                    {
        //                        await _supplierService.GetByNameAsync(supplierName);
        //                    }
        //                    catch (DataNotFoundException)
        //                    {
        //                        suppliersCreated++;
        //                    }
        //                    processedSuppliers.Add(supplierName);
        //                }

        //                var supplierDTO = await _supplierService.GetOrCreateByNameAsync(supplierName);
        //                supplierInfo = (supplierDTO.Id, supplierDTO.Name);
        //                supplierCache[supplierName] = supplierInfo;
        //            }

        //            var medicineName = row.MedicineName!;
        //            if (!medicineIdCache.TryGetValue(medicineName, out var medicineId))
        //            {
        //                var (id, wasCreated) = await this.GetOrCreateMedicineIdAsync(medicineName, row.UnitPrice);
        //                medicineId = id;
        //                medicineIdCache[medicineName] = medicineId;
        //                if (wasCreated)
        //                {
        //                    medicinesCreated++;
        //                }
        //            }

        //            validRows.Add(row);
        //        }

        //        if (firstValidRow == null)
        //        {
        //            response.SuppliersCreated = suppliersCreated;
        //            response.MedicinesCreated = medicinesCreated;
        //            response.ImportsCreated = 0;
        //            response.CreatedImports = [];
        //            await _unitOfWork.CommitTransactionAsync();
        //            return response;
        //        }

        //        var importDetailsByMedicineId = new Dictionary<int, UpsertImportDetailDTO>();
        //        var allMedicineIds = new HashSet<int>();

        //        foreach (var row in validRows)
        //        {
        //            var medicineId = medicineIdCache[row.MedicineName!];
        //            allMedicineIds.Add(medicineId);

        //            if (importDetailsByMedicineId.TryGetValue(medicineId, out var existingDetail))
        //            {
        //                existingDetail.Quantity += row.Quantity;
        //            }
        //            else
        //            {
        //                var importDetailDto = _mapper.Map<UpsertImportDetailDTO>(row);
        //                importDetailDto.MedicineId = medicineId;
        //                importDetailsByMedicineId[medicineId] = importDetailDto;
        //            }
        //        }

        //        var importDetails = importDetailsByMedicineId.Values.ToList();

        //        await this.CheckListMedicineExistByIdsAsync(allMedicineIds.ToList());

        //        var supplierInfoForImport = supplierCache[firstValidRow.SupplierName!];
        //        var createImportDTO = _mapper.Map<CreateImportDTO>(firstValidRow);
        //        createImportDTO.SupplierName = supplierInfoForImport.Name ?? firstValidRow.SupplierName!;
        //        createImportDTO.ImportDetails = importDetails;
        //        createImportDTO.ImportDate ??= DateTime.UtcNow;

        //        var import = _mapper.Map<Import>(createImportDTO);
        //        import.ManagerId = _currentUserService.UserId;
        //        import.SupplierId = supplierInfoForImport.Id;

        //        await this.UpdateInventoriesFromImportAsync(import);
        //        await _repository.AddAsync(import);
        //        await _unitOfWork.SaveChangeAsync();

        //        var createdImport = _mapper.Map<GetImportDTO>(import);
        //        if (import.ImportDetails != null && import.ImportDetails.Any())
        //        {
        //            createdImport.ImportDetails = import.ImportDetails
        //                .Select(d => _mapper.Map<GetImportDetailDTO>(d))
        //                .ToList();
        //        }

        //        await this.RequestValueMedicineNamesAsync([createdImport]);
        //        await this.RequestValueSupplierNamesAsync([createdImport]);

        //        response.SuppliersCreated = suppliersCreated;
        //        response.MedicinesCreated = medicinesCreated;
        //        response.ImportsCreated = 1;
        //        response.CreatedImports = [createdImport];

        //        await _unitOfWork.CommitTransactionAsync();
        //        return response;
        //    }
        //    catch
        //    {
        //        await _unitOfWork.RollbackTransactionAsync();
        //        throw;
        //    }
        //}

        //  private async Task<(int Id, bool WasCreated)> GetOrCreateMedicineIdAsync(string medicineName, double price)
        //  {
        //    var getListMedicineDataByNamesEvent = new GetListMedicineDataByNamesEvent { Names = [medicineName] };
        //    var getListMedicineDataByNamesContract = await _messageBus
        //        .RequestAsync<GetListMedicineDataByNamesEvent, GetListMedicineDataByNamesContract>(
        //            getListMedicineDataByNamesEvent);

        //    var existingMedicine = getListMedicineDataByNamesContract.Data
        //        .FirstOrDefault(m => string.Equals(m.Name, medicineName, StringComparison.OrdinalIgnoreCase));

        //    if (existingMedicine != null)
        //    {
        //      return (existingMedicine.Id, false);
        //    }

        //    var createMedicineItem = _mapper.Map<CreateMedicineFromExcelEventItem>(
        //        new ReadImportRowFromExcelDTO { MedicineName = medicineName, UnitPrice = price });

        //    var createMedicinesEvent = new CreateMedicinesFromExcelEvent
        //    {
        //      CreateMedicineFromExcelEventItems = [createMedicineItem]
        //    };
        //    var createMedicinesContract = await _messageBus
        //        .RequestAsync<CreateMedicinesFromExcelEvent, CreateMedicinesFromExcelContract>(
        //            createMedicinesEvent);

        //    if (createMedicinesContract.Data.Count > 0)
        //    {
        //      return (createMedicinesContract.Data[0].MedicineId, true);
        //    }

        //    throw new InvalidDataException($"Failed to create medicine: {medicineName}");
        //    //}
        //}
    }
}