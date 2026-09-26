using AutoMapper;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using VoucherSvc.BLL.DTOs.VoucherDTOs;
using VoucherSvc.BLL.FilterDTOs;
using VoucherSvc.BLL.Interfaces;
using VoucherSvc.DAL.Enums;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.Implements
{
    public class VoucherService(
        IMongoGenericRepository<Voucher> _voucherRepository,
        IMapper _mapper,
        IUploadFileService _uploadFileService,
        IMessageBus _messageBus) : IVoucherService
    {
        public async Task<GetVoucherDTO> CreateAsync(CreateVoucherDTO dto)
        {
            this.ValidateDiscountRules(dto.VoucherType, dto.DiscountType, dto.DiscountValue, dto.MaxDiscount);

            Voucher voucher;

            switch (dto.VoucherType)
            {
                case nameof(VoucherTypeEnum.Medicine):
                    voucher = _mapper.Map<MedicineDiscount>(dto);
                    ((MedicineDiscount)voucher).MedicineIds = await this.ValidateMedicineVoucherAsync(
                        dto.MedicineIds,
                        dto.MinPurchaseAmount,
                        dto.MaxDiscount);
                    break;

                case nameof(VoucherTypeEnum.Patient):
                    if (dto.Image == null)
                    {
                        throw new ValidationFailureException(nameof(CreateVoucherDTO.Image), "Patient voucher must have an image");
                    }

                    if (dto.Quantity <= 0)
                    {
                        throw new ValidationFailureException(nameof(CreateVoucherDTO.Quantity), "Patient voucher must have a valid quantity");
                    }

                    voucher = _mapper.Map<Voucher>(dto);
                    break;

                default:
                    throw new InvalidDataException($"Invalid voucher type: {dto.VoucherType}");
            }

            this.NormalizeVoucherByRules(voucher);
            this.UpdateVoucherStatusByExpireDate(voucher);

            if (voucher is MedicineDiscount medicineDiscount)
            {
                await this.EnsureNoActiveMedicineDiscountOverlapAsync(medicineDiscount);
            }

            voucher.Code = await this.GenerateUniqueCodeAsync(dto.VoucherType);

            if (dto.Image != null)
            {
                voucher.Image = await _uploadFileService.UploadAsync(dto.Image, typeof(Voucher).Name);
            }

            await _voucherRepository.AddAsync(voucher);

            return _mapper.Map<GetVoucherDTO>(voucher);
        }

        public async Task<GetVoucherDTO> UpdateAsync(string id, UpdateVoucherDTO dto)
        {
            var existingVoucher = await _voucherRepository.GetByIdAsync(id);
            if (existingVoucher == null)
            {
                throw new DataNotFoundException("Voucher not found");
            }

            var originalType = existingVoucher.VoucherType;
            var originalCode = existingVoucher.Code;
            var originalImage = existingVoucher.Image;
            List<int> validatedMedicineIds = [];

            this.ValidateDiscountRules(
                originalType.ToString(),
                dto.DiscountType ?? existingVoucher.DiscountType.ToString(),
                dto.DiscountValue ?? existingVoucher.DiscountValue,
                dto.MaxDiscount);

            if (originalType == VoucherTypeEnum.Medicine)
            {
                validatedMedicineIds = await this.ValidateMedicineVoucherAsync(
                    dto.MedicineIds,
                    dto.MinPurchaseAmount,
                    dto.MaxDiscount);
            }
            else
            {
                if (dto.Quantity.HasValue && dto.Quantity.Value <= 0)
                {
                    throw new ValidationFailureException("Patient voucher must have a valid quantity");
                }
            }

            _mapper.Map(dto, existingVoucher);

            existingVoucher.VoucherType = originalType;
            existingVoucher.Code = originalCode;
            existingVoucher.Image = originalImage;

            if (existingVoucher is MedicineDiscount medicineVoucher)
            {
                medicineVoucher.MedicineIds = validatedMedicineIds;
            }

            this.NormalizeVoucherByRules(existingVoucher);
            this.UpdateVoucherStatusByExpireDate(existingVoucher);

            if (existingVoucher is MedicineDiscount updatedMedicineVoucher)
            {
                await this.EnsureNoActiveMedicineDiscountOverlapAsync(updatedMedicineVoucher, id);
            }

            if (dto.Image is { Length: > 0 } newImage)
            {
                if (!string.IsNullOrWhiteSpace(existingVoucher.Image))
                {
                    await _uploadFileService.DeleteAsync(existingVoucher.Image);
                }

                existingVoucher.Image = await _uploadFileService.UploadAsync(newImage, typeof(Voucher).Name);
            }

            await _voucherRepository.UpdateAsync(id, existingVoucher);

            return _mapper.Map<GetVoucherDTO>(existingVoucher);
        }

        public async Task DeleteAsync(string id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
            {
                throw new DataNotFoundException("Voucher not found");
            }

            if (!string.IsNullOrEmpty(voucher.Image))
            {
                await _uploadFileService.DeleteAsync(voucher.Image);
            }

            await _voucherRepository.RemoveAsync(id);
        }

        public async Task<GetVoucherDTO> GetByIdAsync(string id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
            {
                throw new DataNotFoundException("Voucher not found");
            }

            var voucherDTO = _mapper.Map<GetVoucherDTO>(voucher);

            if (voucher is MedicineDiscount medicineDiscount && medicineDiscount.MedicineIds.Any())
            {
                voucherDTO.Medicines = await this.GetApplicableMedicinesAsync(medicineDiscount.MedicineIds);
                voucherDTO.MedicineIds = voucherDTO.Medicines.Select(m => m.Id).ToList();
            }

            return voucherDTO;
        }

        public async Task<PaginationResult<GetVoucherDTO>> GetAllAsync(VoucherFilterDTO filter)
        {
            var (totalCount, items) = await _voucherRepository.GetAllPaginatedAsync(filter);
            var results = _mapper.Map<List<GetVoucherDTO>>(items);

            var allMedicineIds = results
                .Where(v => v.MedicineIds.Any())
                .SelectMany(v => v.MedicineIds)
                .Distinct()
                .ToList();

            if (allMedicineIds.Any())
            {
                var medicines = await this.GetApplicableMedicinesAsync(allMedicineIds);
                var medicineDict = medicines.ToDictionary(m => m.Id);

                foreach (var voucher in results.Where(v => v.MedicineIds.Any()))
                {
                    voucher.Medicines = voucher.MedicineIds
                        .Where(id => medicineDict.ContainsKey(id))
                        .Select(id => medicineDict[id])
                        .ToList();

                    voucher.MedicineIds = voucher.Medicines.Select(medicine => medicine.Id).ToList();
                }
            }

            return new PaginationResult<GetVoucherDTO>((int)totalCount, filter.PageSize, results);
        }

        public async Task ExpireVouchersAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var expiredVouchers = await _voucherRepository.GetAllAsync(
                v => v.VoucherStatus == VoucherStatusEnum.Active && v.ExpireDate < today
            );

            foreach (var voucher in expiredVouchers)
            {
                voucher.VoucherStatus = VoucherStatusEnum.Expired;
                await _voucherRepository.UpdateAsync(voucher.Id, voucher);
            }
        }

        #region Helper Methods
        private async Task<string> GenerateUniqueCodeAsync(string type)
        {
            string prefix = type switch
            {
                nameof(VoucherTypeEnum.Patient) => "PV",
                nameof(VoucherTypeEnum.Medicine) => "MD",
                _ => throw new InvalidDataException("Invalid voucher type")
            };

            string code;
            bool isUnique;

            do
            {
                string randomPart = StringUtil.GenerateRandomString(9);
                code = $"{prefix}{randomPart}";

                var existingVoucher = await _voucherRepository.GetByConditionAsync(v => v.Code == code);
                isUnique = existingVoucher == null;
            } while (!isUnique);

            return code;
        }

        private void ValidateDiscountRules(string? voucherType, string? discountType, double discountValue, double? maxDiscount)
        {
            if (voucherType == nameof(VoucherTypeEnum.Medicine) &&
                discountType == nameof(DiscountTypeEnum.FixedAmount))
            {
                throw new ValidationFailureException(
                    nameof(UpdateVoucherDTO.DiscountType),
                    "Medicine discount only supports percentage discount type");
            }

            if (discountType == nameof(DiscountTypeEnum.Percentage) && discountValue > 100)
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.DiscountValue), "Percentage discount cannot exceed 100%");
            }

            if (discountType == nameof(DiscountTypeEnum.FixedAmount) && maxDiscount.HasValue)
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.MaxDiscount), "Fixed amount discount cannot have MaxDiscount");
            }
        }

        private async Task<List<int>> ValidateMedicineVoucherAsync(
            IEnumerable<int> medicineIds,
            double? minPurchaseAmount,
            double? maxDiscount)
        {
            var validMedicineIds = (medicineIds ?? [])
                .Where(id => id > 0)
                .ToList();

            if (!validMedicineIds.Any())
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.MedicineIds), "Medicine discount must have at least one applicable medicine");
            }

            if (validMedicineIds.Count != validMedicineIds.Distinct().Count())
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.MedicineIds), "Applicable medicines cannot contain duplicates");
            }

            if (minPurchaseAmount.HasValue)
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.MinPurchaseAmount), "Medicine discount cannot have MinPurchaseAmount");
            }

            if (maxDiscount.HasValue)
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.MaxDiscount), "Medicine discount cannot have MaxDiscount");
            }

            var applicableMedicineIds = await this.GetApplicableMedicineIdsAsync(validMedicineIds);
            if (applicableMedicineIds.Count != validMedicineIds.Count)
            {
                throw new ValidationFailureException(nameof(UpdateVoucherDTO.MedicineIds), "Applicable medicines must be public medicines");
            }

            return applicableMedicineIds;
        }

        private void NormalizeVoucherByRules(Voucher voucher)
        {
            if (voucher.VoucherType == VoucherTypeEnum.Medicine)
            {
                voucher.Quantity = 0;
                voucher.MinPurchaseAmount = 0;
                voucher.MaxDiscount = 0;
            }

            if (voucher.DiscountType == DiscountTypeEnum.FixedAmount)
            {
                voucher.MaxDiscount = 0;
            }
        }

        private void UpdateVoucherStatusByExpireDate(Voucher voucher)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            voucher.VoucherStatus = voucher.ExpireDate < today
                ? VoucherStatusEnum.Expired
                : VoucherStatusEnum.Active;
        }

        private async Task EnsureNoActiveMedicineDiscountOverlapAsync(
            MedicineDiscount voucher,
            string? excludedVoucherId = null)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (voucher.VoucherStatus != VoucherStatusEnum.Active || voucher.ExpireDate < today)
            {
                return;
            }

            var applicableMedicineIds = (await this.GetApplicableMedicineIdsAsync(voucher.MedicineIds))
                .ToHashSet();

            if (applicableMedicineIds.Count == 0)
            {
                return;
            }

            var activeMedicineVouchers = await _voucherRepository.GetAllAsync(v =>
                v.VoucherType == VoucherTypeEnum.Medicine &&
                v.DiscountType == DiscountTypeEnum.Percentage &&
                v.VoucherStatus == VoucherStatusEnum.Active &&
                v.ExpireDate >= today);

            var applicableMedicineIdSetOfExistingVouchers = (await this.GetApplicableMedicineIdsAsync(
                activeMedicineVouchers
                    .OfType<MedicineDiscount>()
                    .SelectMany(existingVoucher => existingVoucher.MedicineIds)))
                .ToHashSet();

            var conflictingDiscounts = activeMedicineVouchers
                .OfType<MedicineDiscount>()
                .Where(existingVoucher => existingVoucher.Id != excludedVoucherId)
                .Select(existingVoucher => new
                {
                    VoucherCode = existingVoucher.Code,
                    ConflictingMedicineIds = existingVoucher.MedicineIds
                        .Where(applicableMedicineIdSetOfExistingVouchers.Contains)
                        .Intersect(applicableMedicineIds)
                        .OrderBy(medicineId => medicineId)
                        .ToList()
                })
                .Where(conflict => conflict.ConflictingMedicineIds.Count > 0)
                .ToList();

            if (!conflictingDiscounts.Any())
            {
                return;
            }

            var conflictingMedicineIds = conflictingDiscounts
                .SelectMany(conflict => conflict.ConflictingMedicineIds)
                .Distinct()
                .OrderBy(medicineId => medicineId);

            var conflictingVoucherCodes = conflictingDiscounts
                .Select(conflict => conflict.VoucherCode)
                .Distinct()
                .OrderBy(code => code);

            throw new DataConflictException(
                $"Applicable medicines [{string.Join(", ", conflictingMedicineIds)}] already belong to active medicine discount(s): {string.Join(", ", conflictingVoucherCodes)}");
        }

        private async Task<List<int>> GetApplicableMedicineIdsAsync(IEnumerable<int> medicineIds)
        {
            var applicableMedicines = await this.GetApplicableMedicineContractsAsync(medicineIds);
            return applicableMedicines.Select(medicine => medicine.Id).ToList();
        }

        private async Task<List<MedicineInfo>> GetApplicableMedicinesAsync(IEnumerable<int> medicineIds)
        {
            var applicableMedicines = await this.GetApplicableMedicineContractsAsync(medicineIds);
            return applicableMedicines.Select(medicine => new MedicineInfo
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Brand = medicine.Brand,
                Price = medicine.Price,
                CategoryName = medicine.CategoryName
            }).ToList();
        }

        private async Task<List<GetMedicineByIdContract>> GetApplicableMedicineContractsAsync(IEnumerable<int> medicineIds)
        {
            var distinctMedicineIds = (medicineIds ?? [])
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (distinctMedicineIds.Count == 0)
            {
                return [];
            }

            var medicineEvent = new GetListMedicineDataByIdsEvent
            {
                Ids = distinctMedicineIds
            };

            var medicineResponse = await _messageBus.RequestAsync<GetListMedicineDataByIdsEvent, GetListMedicineDataByIdsContract>(medicineEvent);
            var applicableMedicineDict = medicineResponse.Data
                .Where(medicine => medicine.IsPublic)
                .ToDictionary(medicine => medicine.Id);

            return distinctMedicineIds
                .Where(applicableMedicineDict.ContainsKey)
                .Select(id => applicableMedicineDict[id])
                .ToList();
        }
        #endregion
    }
}
