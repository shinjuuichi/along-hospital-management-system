using AutoMapper;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using VoucherSvc.BLL.DTOs.PatientVoucherDTOs;
using VoucherSvc.BLL.DTOs.VoucherDTOs;
using VoucherSvc.BLL.FilterDTOs;
using VoucherSvc.BLL.Interfaces;
using VoucherSvc.DAL.Enums;
using VoucherSvc.DAL.Models;

namespace VoucherSvc.BLL.Implements
{
    public class PatientVoucherService(
        IMongoGenericRepository<Voucher> _voucherRepository,
        IMongoGenericRepository<PatientVoucher> _patientVoucherRepository,
        ICurrentUserService _currentUserService,
        IMapper _mapper)
        : IPatientVoucherService
    {
        public async Task CollectVoucherForNewPatientAsync(int patientId)
        {
            var welcomeVouchers = await _voucherRepository.GetAllAsync(v =>
                v.VoucherType == VoucherTypeEnum.Patient &&
                v.VoucherStatus == VoucherStatusEnum.Active &&
                v.ExpireDate > DateOnly.FromDateTime(DateTime.Now) &&
                v.Quantity > 0 &&
                (v.Code.Contains("WELCOME") || v.Code.Contains("FIRST")));

            if (!welcomeVouchers.Any())
            {
                return;
            }

            var welcomeVoucher = welcomeVouchers.First();

            var existingVoucher = await _patientVoucherRepository.GetByConditionAsync(
                pv => pv.PatientId == patientId && pv.VoucherId == welcomeVoucher.Id);

            if (existingVoucher != null)
            {
                return;
            }

            var patientVoucher = new PatientVoucher
            {
                PatientId = patientId,
                VoucherId = welcomeVoucher.Id,
            };
            await _patientVoucherRepository.AddAsync(patientVoucher);

            welcomeVoucher.Quantity -= 1;
            await _voucherRepository.UpdateAsync(welcomeVoucher.Id, welcomeVoucher);
        }

        public async Task<PaginationResult<GetVoucherDTO>> GetCollectibleVouchersAsync(VoucherFilterDTO filter)
        {
            filter.VoucherType = nameof(VoucherTypeEnum.Patient);
            var (totalCount, items) = await _voucherRepository.GetAllPaginatedAsync(filter);

            if (_currentUserService.IsAuthenticated)
            {
                var collectedVoucherIds = await _patientVoucherRepository.GetAllAsync(
                    pv => pv.PatientId == _currentUserService.UserId
                );
                var collectedIds = collectedVoucherIds.Select(pv => pv.VoucherId).ToHashSet();

                items = items.Where(v => !collectedIds.Contains(v.Id)).ToList();
                totalCount = items.Count;
            }

            var voucherDtos = _mapper.Map<List<GetVoucherDTO>>(items);

            return new PaginationResult<GetVoucherDTO>(totalCount, filter.PageSize, voucherDtos);
        }

        public async Task SelfCollectVoucherAsync(CollectVoucherDTO request)
        {
            var voucher = await _voucherRepository.GetByConditionAsync(v => v.Code == request.VoucherCode);

            if (voucher == null)
            {
                throw new DataNotFoundException($"Voucher with code {request.VoucherCode} not found");
            }

            if (voucher.VoucherType != VoucherTypeEnum.Patient)
            {
                throw new ValidationFailureException("Only patient vouchers can be collected. Medicine discounts are applied automatically at checkout.");
            }

            if (voucher.VoucherStatus != VoucherStatusEnum.Active)
            {
                throw new ValidationFailureException("Voucher is not active");
            }

            if (voucher.ExpireDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ValidationFailureException("Voucher has expired");
            }

            if (voucher.Quantity <= 0)
            {
                throw new ValidationFailureException("Voucher is out of stock");
            }

            var existingVoucher = await _patientVoucherRepository.GetByConditionAsync(
                pv => pv.PatientId == _currentUserService.UserId && pv.VoucherId == voucher.Id
            );

            if (existingVoucher != null)
            {
                throw new ValidationFailureException("You have already collected this voucher");
            }

            var patientVoucher = new PatientVoucher
            {
                PatientId = _currentUserService.UserId,
                VoucherId = voucher.Id,
            };
            await _patientVoucherRepository.AddAsync(patientVoucher);

            voucher.Quantity -= 1;
            await _voucherRepository.UpdateAsync(voucher.Id, voucher);
        }

        public async Task<PaginationResult<GetMyPatientVoucherDTO>> GetMyVouchersAsync(PatientVoucherFilterDTO filter)
        {
            filter.PatientId = _currentUserService.UserId;

            var joinedResults = await _patientVoucherRepository.GetWithJoinAsync<Voucher, PatientVoucherWithVoucher>(
                pv => pv.PatientId == _currentUserService.UserId && !pv.IsUsed,
                pv => pv.VoucherId,
                v => v.Id,
                (pv, v) => new PatientVoucherWithVoucher { PatientVoucher = pv, Voucher = v }
            );

            var voucherDtos = joinedResults
                .Where(result => result.Voucher.ExpireDate >= DateOnly.FromDateTime(DateTime.Now))
                .Select(result => _mapper.Map<GetMyPatientVoucherDTO>(result.Voucher))
                .ToList();

            return new PaginationResult<GetMyPatientVoucherDTO>(voucherDtos.Count, filter.PageSize, voucherDtos);
        }

        public async Task<List<GetMyPatientVoucherDTO>> GetAllMyVouchersAsync()
        {
            var joinedResults = await _patientVoucherRepository.GetWithJoinAsync<Voucher, PatientVoucherWithVoucher>(
                pv => pv.PatientId == _currentUserService.UserId && !pv.IsUsed,
                pv => pv.VoucherId,
                v => v.Id,
                (pv, v) => new PatientVoucherWithVoucher { PatientVoucher = pv, Voucher = v }
            );

            var voucherDtos = joinedResults
                .Where(result => result.Voucher.ExpireDate >= DateOnly.FromDateTime(DateTime.Now))
                .Select(result => _mapper.Map<GetMyPatientVoucherDTO>(result.Voucher))
                .ToList();

            return voucherDtos;
        }
    }
}
