using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using StaffSvc.BLL.DTOs.StaffCertificates;
using StaffSvc.BLL.Interfaces;
using StaffSvc.BLL.StateMachines;
using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.Implements
{
    public class StaffCertificateService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        IStaffService staffService)
            : BaseService<StaffCertificate, UpsertStaffCertificateDTO, UpsertStaffCertificateDTO, GetStaffCertificateDTO>(
                unitOfWork,
                mapper,
                includes: [
                    nameof(StaffCertificate.StaffCertificateType),
                    nameof(StaffCertificate.Staff)]),
                        IStaffCertificateService
    {
        private const string DEFAULT_AUTH_STATUS_VERIFIED = "Verified";
        private const string DEFAULT_AUTH_STATUS_SUSPENDED = "Suspended";

        private readonly IMessageBus _messageBus = messageBus;
        private readonly IStaffService _staffService = staffService;

        public override async Task<GetStaffCertificateDTO> GetByIdAsync(int id)
        {
            var staffCertificate = await base.GetByIdAsync(id);
            await this.GetStaffNameAsync([staffCertificate]);

            return staffCertificate;
        }

        public override async Task<List<GetStaffCertificateDTO>> GetAllAsync()
        {
            var staffCertificates = await base.GetAllAsync();
            await this.GetStaffNameAsync(staffCertificates);

            return staffCertificates;
        }

        public override async Task<PaginationResult<GetStaffCertificateDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO);
            await this.GetStaffNameAsync(result.Collection);

            return result;
        }

        public async Task SuspendCertificatesAsync(int id, string reason)
        {
            var staffCertificate = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(StaffCertificate), id);

            staffCertificate.Reason = reason;
            await this.UpdateStatusAsync(staffCertificate, StaffCertificateStatusEnum.Suspended);
        }

        public async Task ActiveCertificatesAsync(int id, DateOnly expiredDate)
        {
            var staffCertificate = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(StaffCertificate), id);

            staffCertificate.ExpiredDate = expiredDate;
            await this.UpdateStatusAsync(staffCertificate, StaffCertificateStatusEnum.Valid);
        }

        public async Task ApproveCertificatesAsync(int id)
        {
            var staffCertificate = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(StaffCertificate), id);

            staffCertificate.Reason = null;
            await this.UpdateStatusAsync(staffCertificate, StaffCertificateStatusEnum.Valid);
        }

        public async Task ExpireCertificatesAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expiredCertificates = await _repository.GetAllAsync(
                x => x.Status == StaffCertificateStatusEnum.Valid && x.ExpiredDate < today);

            if (expiredCertificates.Count == 0)
            {
                return;
            }

            foreach (var cert in expiredCertificates)
            {
                await this.UpdateStatusAsync(cert, StaffCertificateStatusEnum.Expired);
            }
        }

        public async Task SendExpirationReminderEmailsAsync()
        {
            const int expirationDays = 30;
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expirationDate = today.AddDays(expirationDays);

            var expiringCertificates = await _repository.GetAllAsync(
                x => x.Status == StaffCertificateStatusEnum.Valid
                    && x.ExpiredDate >= today
                    && x.ExpiredDate <= expirationDate, includes: _includes);

            if (expiringCertificates.Count == 0)
            {
                return;
            }

            var staffIds = expiringCertificates.Select(c => c.StaffId).Distinct().ToList();

            var getListAuthContract = await _messageBus.RequestAsync<
                GetListAuthDataByUserIdsEvent,
                GetListAuthDataByUserIdsContract>(
                new GetListAuthDataByUserIdsEvent { UserIds = staffIds });

            var staffEmailDict = getListAuthContract.Data.ToDictionary(s => s.UserId);

            var getListUserContract = await _messageBus.RequestAsync<
                GetListUserDataByUserIdsEvent,
                GetListUserDataByUserIdsContract>(
                new GetListUserDataByUserIdsEvent { UserIds = staffIds });

            var staffNameDict = getListUserContract.Data.ToDictionary(s => s.UserId);

            foreach (var cert in expiringCertificates)
            {
                if (!staffEmailDict.TryGetValue(cert.StaffId, out var staffAuthData) || string.IsNullOrWhiteSpace(staffAuthData.Email))
                {
                    continue;
                }

                var staffName = staffNameDict.TryGetValue(cert.StaffId, out var staffUserData) ? staffUserData.Name : "Staff";
                var daysUntilExpiration = cert.ExpiredDate.DayNumber - today.DayNumber;

                var emailEvent = new SendCertificateExpirationReminderEmailEvent
                {
                    Email = staffAuthData.Email,
                    StaffName = staffName,
                    CertificateName = cert.StaffCertificateType?.Name ?? "Certificate",
                    CertificateNo = cert.CertificateNo,
                    ExpiredDate = cert.ExpiredDate,
                    DaysUntilExpiration = daysUntilExpiration
                };

                await _messageBus.PublishAsync(emailEvent);
            }
        }

        private async Task GetStaffNameAsync(List<GetStaffCertificateDTO> staffCertificates)
        {
            var staffIds = staffCertificates.Select(c => c.StaffId).Distinct().ToList();
            if (staffIds.Count == 0)
            {
                return;
            }

            var staffDataList = await _messageBus.RequestAsync<
                GetListUserDataByUserIdsEvent,
                GetListUserDataByUserIdsContract>(
                new GetListUserDataByUserIdsEvent { UserIds = staffIds });

            var staffDict = staffDataList.Data.ToDictionary(s => s.UserId);

            foreach (var staffCert in staffCertificates)
            {
                if (staffDict.TryGetValue(staffCert.StaffId, out var staffData))
                {
                    staffCert.StaffName = staffData.Name;
                }
            }
        }

        private async Task UpdateStatusAsync(StaffCertificate staffCertificate, StaffCertificateStatusEnum newStatus)
        {
            var stateMachine = new StaffCertificateStateMachine(staffCertificate);
            var oldStatus = staffCertificate.Status;

            if (!stateMachine.CanFire(newStatus))
            {
                throw new InvalidDataException($"Cannot transition certificate from '{oldStatus}' to '{newStatus}'");
            }

            try
            {
                stateMachine.Fire(newStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change certificate status: {e.Message}");
            }

            ChangeAuthStatusEvent? changeAuthStatusEvent = null;
            if (newStatus is StaffCertificateStatusEnum.Suspended)
            {
                await _staffService.UpdateStatusAsync([staffCertificate.StaffId], StaffStatusEnum.Suspended);
                changeAuthStatusEvent = new ChangeAuthStatusEvent
                {
                    UserId = staffCertificate.StaffId,
                    Status = DEFAULT_AUTH_STATUS_SUSPENDED
                };
            }
            else if (oldStatus is StaffCertificateStatusEnum.Suspended && newStatus is StaffCertificateStatusEnum.Valid)
            {
                await _staffService.UpdateStatusAsync([staffCertificate.StaffId], StaffStatusEnum.Active);
                changeAuthStatusEvent = new ChangeAuthStatusEvent
                {
                    UserId = staffCertificate.StaffId,
                    Status = DEFAULT_AUTH_STATUS_VERIFIED
                };
            }

            _repository.Update(staffCertificate);
            await _unitOfWork.SaveChangeAsync();
            if (changeAuthStatusEvent is not null)
            {
                await _messageBus.PublishAsync(changeAuthStatusEvent);
            }
        }
    }
}
