using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using MongoDB.Driver;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using VoucherSvc.BLL.DTOs.DiscountDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.ApplyVoucherDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewListMedicineDiscountDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewVoucherDTOs;
using VoucherSvc.BLL.Interfaces;
using VoucherSvc.DAL.Enums;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.Implements
{
    public class DiscountService(
        IMongoGenericRepository<Voucher> _voucherRepository,
        IMongoGenericRepository<PatientVoucher> _patientVoucherRepository,
        IMessageBus _messageBus) : IDiscountService
    {
        public async Task<ApplyVoucherResponseDTO> ApplyVoucherAsync(ApplyVoucherRequestDTO request)
        {
            var originalTotal = request.Medicines.Sum(m => m.Price * m.Quantity);
            var discountedTotal = originalTotal;

            var medicineDiscounts = await this.GetActiveMedicineDiscountsAsync();
            var publicMedicineIds = await this.GetApplicableMedicineIdsAsync(request.Medicines.Select(medicine => medicine.MedicineId));
            var (medicineDiscountAmount, medicineDiscountDetails) = this.ApplyMedicineDiscountsAsync(
                request.Medicines,
                medicineDiscounts,
                publicMedicineIds);
            discountedTotal -= medicineDiscountAmount;

            double patientVoucherDiscountAmount = 0;
            string? appliedVoucherCode = null;

            if (!string.IsNullOrWhiteSpace(request.VoucherCode))
            {
                patientVoucherDiscountAmount = await this.ApplyPatientVoucherAsync(
                    request.PatientId,
                    request.VoucherCode,
                    discountedTotal);
                discountedTotal -= patientVoucherDiscountAmount;
                appliedVoucherCode = request.VoucherCode;
            }

            discountedTotal = Math.Max(0, discountedTotal);

            return new ApplyVoucherResponseDTO
            {
                OriginalTotalPrice = originalTotal,
                FinalTotalPrice = discountedTotal,
                MedicineDiscounts = medicineDiscountDetails,
                PatientVoucherDiscountAmount = patientVoucherDiscountAmount,
                PatientVoucherCode = appliedVoucherCode
            };
        }

        public async Task<MedicineDiscountDetailDTO> PreviewMedicineDiscountAsync(PreviewMedicineItemDTO request)
        {
            var medicineDiscounts = await this.GetActiveMedicineDiscountsAsync();
            var originalPrice = request.Price;
            var publicMedicineIds = await this.GetApplicableMedicineIdsAsync([request.MedicineId]);

            if (!publicMedicineIds.Contains(request.MedicineId))
            {
                return new MedicineDiscountDetailDTO
                {
                    MedicineId = request.MedicineId,
                    SKUCode = request.SKUCode,
                    OriginalPrice = originalPrice,
                    MedicineDiscountAmount = 0,
                    FinalPrice = originalPrice
                };
            }

            var applicableDiscounts = medicineDiscounts
                .OfType<MedicineDiscount>()
                .Where(v => v.MedicineIds.Contains(request.MedicineId))
                .ToList();

            double discountAmount = 0;

            if (applicableDiscounts.Any())
            {
                discountAmount = applicableDiscounts
                    .Max(discount => this.CalculateDiscount(
                        originalPrice,
                        discount.DiscountType,
                        discount.DiscountValue,
                        null));
            }

            return new MedicineDiscountDetailDTO
            {
                MedicineId = request.MedicineId,
                SKUCode = request.SKUCode,
                OriginalPrice = originalPrice,
                MedicineDiscountAmount = discountAmount,
                FinalPrice = Math.Max(0, originalPrice - discountAmount)
            };
        }

        public async Task<PreviewListMedicineDiscountResponseDTO> PreviewListMedicineDiscountAsync(PreviewListMedicineDiscountRequestDTO request)
        {
            var medicineDiscounts = await this.GetActiveMedicineDiscountsAsync();
            var publicMedicineIds = await this.GetApplicableMedicineIdsAsync(request.Medicines.Select(medicine => medicine.MedicineId));

            double totalDiscount = 0;
            var details = new List<MedicineDiscountDetailDTO>();

            foreach (var medicine in request.Medicines)
            {
                var originalPrice = medicine.Price;

                if (!publicMedicineIds.Contains(medicine.MedicineId))
                {
                    details.Add(new MedicineDiscountDetailDTO
                    {
                        MedicineId = medicine.MedicineId,
                        SKUCode = medicine.SKUCode,
                        OriginalPrice = originalPrice,
                        MedicineDiscountAmount = 0,
                        FinalPrice = originalPrice
                    });
                    continue;
                }

                var applicableDiscounts = medicineDiscounts
                    .OfType<MedicineDiscount>()
                    .Where(v => v.MedicineIds.Contains(medicine.MedicineId))
                    .ToList();

                double discountAmount = 0;

                if (applicableDiscounts.Any())
                {
                    discountAmount = applicableDiscounts
                        .Max(discount => this.CalculateDiscount(
                            originalPrice,
                            discount.DiscountType,
                            discount.DiscountValue,
                            null));

                    totalDiscount += discountAmount;
                }

                details.Add(new MedicineDiscountDetailDTO
                {
                    MedicineId = medicine.MedicineId,
                    SKUCode = medicine.SKUCode,
                    OriginalPrice = originalPrice,
                    MedicineDiscountAmount = discountAmount,
                    FinalPrice = Math.Max(0, originalPrice - discountAmount)
                });
            }

            return new PreviewListMedicineDiscountResponseDTO
            {
                MedicineDiscounts = details
            };
        }

        public async Task<PreviewVoucherResponseDTO> PreviewVoucherAsync(PreviewVoucherRequestDTO request)
        {
            var originalTotal = request.Medicines.Sum(m => m.Price * m.Quantity);
            var discountedTotal = originalTotal;

            var medicineDiscounts = await this.GetActiveMedicineDiscountsAsync();
            var publicMedicineIds = await this.GetApplicableMedicineIdsAsync(request.Medicines.Select(medicine => medicine.MedicineId));
            var (medicineDiscountAmount, medicineDiscountDetails) = this.ApplyMedicineDiscountsAsync(
                request.Medicines,
                medicineDiscounts,
                publicMedicineIds);
            discountedTotal -= medicineDiscountAmount;

            double patientVoucherDiscountAmount = 0;
            string? appliedVoucherCode = null;

            if (!string.IsNullOrWhiteSpace(request.VoucherCode))
            {
                patientVoucherDiscountAmount = await this.PreviewPatientVoucherAsync(
                    request.PatientId,
                    request.VoucherCode,
                    discountedTotal);
                discountedTotal -= patientVoucherDiscountAmount;
                appliedVoucherCode = request.VoucherCode;
            }

            discountedTotal = Math.Max(0, discountedTotal);

            return new PreviewVoucherResponseDTO
            {
                OriginalTotalPrice = originalTotal,
                FinalTotalPrice = discountedTotal,
                MedicineDiscounts = medicineDiscountDetails,
                PatientVoucherDiscountAmount = patientVoucherDiscountAmount,
                PatientVoucherCode = appliedVoucherCode
            };
        }

        #region Shared Helper Methods
        private async Task<double> PreviewPatientVoucherAsync(
          int patientId,
          string voucherCode,
          double currentTotal)
        {
            var voucher = await _voucherRepository.GetByConditionAsync(v => v.Code == voucherCode);
            if (voucher == null)
            {
                throw new DataNotFoundException($"Voucher with code '{voucherCode}' not found");
            }

            var patientVoucher = await _patientVoucherRepository.GetByConditionAsync(pv =>
                pv.PatientId == patientId &&
                pv.VoucherId == voucher.Id &&
                !pv.IsUsed);

            if (patientVoucher == null)
            {
                throw new DataNotFoundException($"You have not collected this voucher or it has already been used");
            }

            if (voucher.VoucherStatus != VoucherStatusEnum.Active)
            {
                throw new DataConflictException("Voucher is not active");
            }

            if (voucher.ExpireDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new DataConflictException("Voucher has expired");
            }

            if (voucher.MinPurchaseAmount > 0 && currentTotal < voucher.MinPurchaseAmount)
            {
                throw new DataConflictException(
                    $"Minimum purchase amount of {voucher.MinPurchaseAmount} is required");
            }

            var discount = this.CalculateDiscount(
                currentTotal,
                voucher.DiscountType,
                voucher.DiscountValue,
                voucher.MaxDiscount);

            return discount;
        }

        private async Task<double> ApplyPatientVoucherAsync(
        int patientId,
        string voucherCode,
        double currentTotal)
        {
            var voucher = await _voucherRepository.GetByConditionAsync(v => v.Code == voucherCode);
            if (voucher == null)
            {
                throw new DataNotFoundException($"Voucher with code '{voucherCode}' not found");
            }

            var patientVoucher = await _patientVoucherRepository.GetByConditionAsync(pv =>
                pv.PatientId == patientId &&
                pv.VoucherId == voucher.Id &&
                !pv.IsUsed);

            if (patientVoucher == null)
            {
                throw new DataNotFoundException($"You have not collected this voucher or it has already been used");
            }

            if (voucher.VoucherStatus != VoucherStatusEnum.Active)
            {
                throw new DataConflictException("Voucher is not active");
            }

            if (voucher.ExpireDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new DataConflictException("Voucher has expired");
            }

            if (voucher.MinPurchaseAmount > 0 && currentTotal < voucher.MinPurchaseAmount)
            {
                throw new DataConflictException(
                    $"Minimum purchase amount of {voucher.MinPurchaseAmount} is required");
            }

            var discount = this.CalculateDiscount(
                currentTotal,
                voucher.DiscountType,
                voucher.DiscountValue,
                voucher.MaxDiscount);

            patientVoucher.IsUsed = true;
            patientVoucher.UsedAt = DateTime.UtcNow;
            await _patientVoucherRepository.UpdateAsync(patientVoucher.Id, patientVoucher);

            return discount;
        }

        private async Task<List<Voucher>> GetActiveMedicineDiscountsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _voucherRepository.GetAllAsync(v =>
                v.VoucherType == VoucherTypeEnum.Medicine &&
                v.DiscountType == DiscountTypeEnum.Percentage &&
                v.VoucherStatus == VoucherStatusEnum.Active &&
                v.ExpireDate >= today);
        }

        private (double TotalDiscount, List<MedicineDiscountDetailDTO> Details) ApplyMedicineDiscountsAsync(
            List<MedicineItemDTO> medicines,
            List<Voucher> medicineDiscountVouchers,
            HashSet<int> publicMedicineIds)
        {
            double totalDiscount = 0;
            var details = new List<MedicineDiscountDetailDTO>();

            foreach (var medicine in medicines)
            {
                var originalPrice = medicine.Price * medicine.Quantity;

                if (!publicMedicineIds.Contains(medicine.MedicineId))
                {
                    details.Add(new MedicineDiscountDetailDTO
                    {
                        MedicineId = medicine.MedicineId,
                        SKUCode = medicine.SKUCode,
                        OriginalPrice = originalPrice,
                        MedicineDiscountAmount = 0,
                        FinalPrice = originalPrice
                    });
                    continue;
                }

                var applicableDiscounts = medicineDiscountVouchers
                    .OfType<MedicineDiscount>()
                    .Where(v => v.MedicineIds.Contains(medicine.MedicineId))
                    .ToList();

                double discountAmount = 0;

                if (applicableDiscounts.Any())
                {
                    discountAmount = applicableDiscounts
                        .Max(discount => this.CalculateDiscount(
                            originalPrice,
                            discount.DiscountType,
                            discount.DiscountValue,
                            null));

                    totalDiscount += discountAmount;
                }

                details.Add(new MedicineDiscountDetailDTO
                {
                    MedicineId = medicine.MedicineId,
                    SKUCode = medicine.SKUCode,
                    OriginalPrice = originalPrice,
                    MedicineDiscountAmount = discountAmount,
                    FinalPrice = Math.Max(0, originalPrice - discountAmount)
                });
            }

            return (totalDiscount, details);
        }

        private async Task<HashSet<int>> GetApplicableMedicineIdsAsync(IEnumerable<int> medicineIds)
        {
            var distinctMedicineIds = medicineIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (distinctMedicineIds.Count == 0)
            {
                return [];
            }

            var medicineResponse = await _messageBus.RequestAsync<GetListMedicineDataByIdsEvent, GetListMedicineDataByIdsContract>(
                new GetListMedicineDataByIdsEvent
                {
                    Ids = distinctMedicineIds
                });

            return medicineResponse.Data
                .Where(medicine => medicine.IsPublic)
                .Select(medicine => medicine.Id)
                .ToHashSet();
        }

        private double CalculateDiscount(
            double amount,
            DiscountTypeEnum discountType,
            double discountValue,
            double? maxDiscount)
        {
            double discount;

            if (discountType == DiscountTypeEnum.Percentage)
            {
                discount = amount * (discountValue / 100);
                if (maxDiscount.HasValue)
                {
                    discount = Math.Min(discount, maxDiscount.Value);
                }
            }
            else
            {
                discount = discountValue;
            }

            return Math.Min(discount, amount);
        }
        #endregion
    }
}
