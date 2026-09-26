using AutoMapper;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs;
using MedicalHistorySvc.BLL.FilterDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.BLL.Utils;
using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.MedicalOrderContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.BillingEvents;
using MessageBroker.Events.InPatientResourceEvents;
using MessageBroker.Events.MedicalOrderEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Extensions;
using SharedLibrary.Services.Interfaces;
using System.Linq.Expressions;

namespace MedicalHistorySvc.BLL.Implements
{
    public class MedicalHistoryQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
            : IMedicalHistoryQueryService
    {
        private const string ACTIVE_OCCUPANCY_STATUS = "Active";

        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        private readonly IGenericRepository<MedicalHistory> _medicalHistoryRepository = unitOfWork.Repository<MedicalHistory>();
        private readonly string[] _includesAll = [
            $"{nameof(MedicalHistory.Complaint)}",
            $"{nameof(Prescription)}.{nameof(Prescription.PrescriptionDetails)}"];

        #region Get All
        public async Task<PaginationResult<GetMedicalHistoryDTO>> GetAllAsync(MedicalHistoryFilterDTO medicalHistoryFilterDTO)
        {
            Expression<Func<MedicalHistory, bool>> filterExpr = await this.ApplyPatientAndDoctorFiltersAsync(
                _ => true,
                medicalHistoryFilterDTO.PatientName,
                medicalHistoryFilterDTO.DoctorName);

            var (total, medicalHistories) = await _medicalHistoryRepository.GetAllPaginatedAsync(
                filterExpr,
                medicalHistoryFilterDTO.Filter,
                medicalHistoryFilterDTO.Sort,
                medicalHistoryFilterDTO.Page,
                medicalHistoryFilterDTO.PageSize,
                _includesAll);

            Dictionary<GetMedicalHistoryDTO, MedicalHistory> medicalHistoryDict
                = medicalHistories.ToDictionary(_mapper.Map<GetMedicalHistoryDTO>);

            await this.RequestValueForMedicalHistoryDTOsAsync(medicalHistoryDict);
            this.FilterComplaintByCurrentUserRole(medicalHistoryDict.Keys.ToList());

            return new PaginationResult<GetMedicalHistoryDTO>(
                total,
                medicalHistoryFilterDTO.PageSize,
                medicalHistoryDict.Keys.ToList());
        }

        public async Task<PaginationResult<GetMedicalHistoryDTO>> GetAllByDoctorIdAsync(int doctorId, MedicalHistoryFilterDTO medicalHistoryFilterDTO)
        {
            Expression<Func<MedicalHistory, bool>> filterExpr = await this.ApplyPatientAndDoctorFiltersAsync(
               mh => mh.DoctorId == doctorId,
               medicalHistoryFilterDTO.PatientName,
               null);

            var (total, medicalHistories) = await _medicalHistoryRepository.GetAllPaginatedAsync(
                filterExpr,
                medicalHistoryFilterDTO.Filter,
                medicalHistoryFilterDTO.Sort,
                medicalHistoryFilterDTO.Page,
                medicalHistoryFilterDTO.PageSize,
                _includesAll);

            Dictionary<GetMedicalHistoryDTO, MedicalHistory> medicalHistoryDict
                = medicalHistories.ToDictionary(_mapper.Map<GetMedicalHistoryDTO>);

            await this.RequestValueForMedicalHistoryDTOsAsync(medicalHistoryDict);
            this.FilterComplaintByCurrentUserRole(medicalHistoryDict.Keys.ToList());

            return new PaginationResult<GetMedicalHistoryDTO>(
                total,
                medicalHistoryFilterDTO.PageSize,
                medicalHistoryDict.Keys.ToList());
        }

        public async Task<PaginationResult<GetMedicalHistoryDTO>> GetAllByPatientIdAsync(int patientId, MedicalHistoryFilterDTO medicalHistoryFilterDTO)
        {
            Expression<Func<MedicalHistory, bool>> filterExpr = await this.ApplyPatientAndDoctorFiltersAsync(
                mh => mh.PatientId == patientId
                    && mh.MedicalHistoryStatus == MedicalHistoryStatusEnum.Completed,
                null,
                medicalHistoryFilterDTO.DoctorName);

            var (total, medicalHistories) = await _medicalHistoryRepository.GetAllPaginatedAsync(
                filterExpr,
                medicalHistoryFilterDTO.Filter,
                medicalHistoryFilterDTO.Sort,
                medicalHistoryFilterDTO.Page,
                medicalHistoryFilterDTO.PageSize,
                _includesAll);

            Dictionary<GetMedicalHistoryDTO, MedicalHistory> medicalHistoryDict
                = medicalHistories.ToDictionary(_mapper.Map<GetMedicalHistoryDTO>);

            await this.RequestValueForMedicalHistoryDTOsAsync(medicalHistoryDict);
            this.FilterComplaintByCurrentUserRole(medicalHistoryDict.Keys.ToList());

            return new PaginationResult<GetMedicalHistoryDTO>(
                total,
                medicalHistoryFilterDTO.PageSize,
                medicalHistoryDict.Keys.ToList());
        }

        public async Task<List<GetMedicalHistoryDTO>> GetAllPendingAsync()
        {
            var medicalHistories = await _medicalHistoryRepository.GetAllAsync(mh => mh.MedicalHistoryStatus == MedicalHistoryStatusEnum.PendingPayment);

            Dictionary<GetMedicalHistoryDTO, MedicalHistory> medicalHistoryDict
                = medicalHistories.ToDictionary(_mapper.Map<GetMedicalHistoryDTO>);
            await this.RequestValueForMedicalHistoryDTOsAsync(medicalHistoryDict);

            return medicalHistoryDict.Keys.ToList();
        }

        public async Task<List<GetMedicalHistoryDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var medicalHistories = await _medicalHistoryRepository.GetAllAsync(mh => ids.Contains(mh.Id));
            return _mapper.Map<List<GetMedicalHistoryDTO>>(medicalHistories);
        }

        public async Task<List<GetMedicalHistoryDTO>> GetAllOccupancySummariesByIdsAsync(List<int> ids)
        {
            if (ids.Count == 0)
            {
                return [];
            }

            Dictionary<int, int> idOrderLookup = [];
            for (var index = 0; index < ids.Count; index++)
            {
                if (!idOrderLookup.ContainsKey(ids[index]))
                {
                    idOrderLookup[ids[index]] = index;
                }
            }

            var medicalHistories = await _medicalHistoryRepository.GetAllAsync(mh => ids.Contains(mh.Id));

            Dictionary<GetMedicalHistoryDTO, MedicalHistory> medicalHistoryDict
                = medicalHistories.ToDictionary(_mapper.Map<GetMedicalHistoryDTO>);

            await this.RequestValueForMedicalHistoryDTOsAsync(medicalHistoryDict);

            return medicalHistoryDict.Keys
                .OrderBy(dto => idOrderLookup.TryGetValue(dto.Id, out var orderIndex)
                    ? orderIndex
                    : int.MaxValue)
                .ToList();
        }
        #endregion

        #region Get One
        public async Task<GetMedicalHistoryDTO> GetByIdAsync(int id, bool includeDataFromAnotherService = true)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(id, _includesAll);
            if (medicalHistory == null)
            {
                throw new DataNotFoundException(typeof(MedicalHistory), id);
            }

            var medicalHistoryDTO = _mapper.Map<GetMedicalHistoryDTO>(medicalHistory);
            this.FilterComplaintByCurrentUserRole([medicalHistoryDTO]);

            if (includeDataFromAnotherService)
            {
                await this.RequestValueForMedicalHistoryDTOsAsync(new() { [medicalHistoryDTO] = medicalHistory });
                await this.RequestValueForSpecificMedicalHistoryDTOAsync(medicalHistoryDTO);
            }

            return medicalHistoryDTO;
        }

        public async Task<bool> CheckExistByIdAsync(int id)
        {
            return await _medicalHistoryRepository.AnyAsync(mh => mh.Id == id);
        }
        #endregion

        #region Request Value For DTO
        private async Task RequestValueForMedicalHistoryDTOsAsync(
            Dictionary<GetMedicalHistoryDTO, MedicalHistory> medicalHistoryDict)
        {
            // Distinct PatientIds, DoctorIds, SpecialtyIds
            var distinctPatientEventIds = medicalHistoryDict
                .Where(mhDict => mhDict.Value.MedicalHistoryStatus != MedicalHistoryStatusEnum.Completed
                    || mhDict.Value.PatientSnapshot == null)
                .Select(mhDict => mhDict.Key.PatientId)
                .Distinct().ToList();
            var distinctDoctorEventIds = medicalHistoryDict.Select(mh => mh.Key.DoctorId).OfType<int>().Distinct().ToList();
            var distinctSpecialtyIds = medicalHistoryDict.Select(mh => mh.Key.SpecialtyId).Distinct().ToList();

            Dictionary<int, GetPatientDataByUserIdContract> patientContractDict = [];
            Dictionary<int, GetStaffDataByUserIdContract> staffContractDict = [];
            Dictionary<int, GetSpecialtyByIdContract> specialtyContractDict = [];

            // Request contracts data by IDs
            if (distinctPatientEventIds.Count > 0)
            {
                var getListPatientDataByIdsEvent = new GetListPatientDataByUserIdsEvent { UserIds = distinctPatientEventIds };
                var getListPatientDataByIdsContract = await _messageBus
                    .RequestAsync<GetListPatientDataByUserIdsEvent, GetListPatientDataByUserIdsContract>(getListPatientDataByIdsEvent);

                patientContractDict = getListPatientDataByIdsContract.Data.ToDictionary(patient => patient.UserId);
            }

            if (distinctDoctorEventIds.Count > 0)
            {
                var getListStaffDataByIdsEvent = new GetListStaffDataByUserIdsEvent { UserIds = distinctDoctorEventIds };
                var getListStaffDataByIdsContract = await _messageBus
                    .RequestAsync<GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>(getListStaffDataByIdsEvent);

                staffContractDict = getListStaffDataByIdsContract.Data.ToDictionary(doctor => doctor.UserId);
            }

            if (distinctSpecialtyIds.Count > 0)
            {
                var getListSpecialtyByIdsEvent = new GetListSpecialtyDataByIdsEvent { SpecialtyIds = distinctSpecialtyIds };
                var getListSpecialtyByIdsContract = await _messageBus
                    .RequestAsync<GetListSpecialtyDataByIdsEvent, GetListSpecialtyDataByIdsContract>(getListSpecialtyByIdsEvent);

                specialtyContractDict = getListSpecialtyByIdsContract.Data.ToDictionary(specialty => specialty.Id);
            }

            // Map contract data to DTO in medicalHistoryDict
            foreach (var (dto, entity) in medicalHistoryDict)
            {
                if (entity.MedicalHistoryStatus == MedicalHistoryStatusEnum.Completed
                    && entity.PatientSnapshot != null)
                {
                    dto.Patient = _mapper.Map<GetMedicalHistoryPatientDTO>(entity.PatientSnapshot);
                }
                else
                {
                    if (patientContractDict.TryGetValue(dto.PatientId, out var patientContract))
                    {
                        dto.Patient = _mapper.Map<GetMedicalHistoryPatientDTO>(patientContract);
                    }
                }

                if (dto.DoctorId.HasValue && staffContractDict.TryGetValue(dto.DoctorId.Value, out var staffContract))
                {
                    dto.Doctor = _mapper.Map<GetMedicalHistoryStaffDTO>(staffContract);
                }

                if (specialtyContractDict.TryGetValue(dto.SpecialtyId, out var specialtyContract))
                {
                    dto.Specialty = _mapper.Map<GetMedicalHistorySpecialtyDTO>(specialtyContract);
                }
            }
        }

        private async Task RequestValueForSpecificMedicalHistoryDTOAsync(GetMedicalHistoryDTO medicalHistoryDTO)
        {
            var getAllInvoicesByMedicalHistoryIdContract = await _messageBus.RequestAsync<
                    GetListInvoiceDataByMedicalHistoryIdEvent,
                    GetListInvoiceDataContract>(new() { MedicalHistoryId = medicalHistoryDTO.Id });

            var getAllMedicalOrdersByMedicalHistoryIdContract = await _messageBus.RequestAsync<
                    GetListMedicalOrderDataByMedicalHistoryIdEvent,
                    GetListMedicalOrderDataContract>(new() { MedicalHistoryId = medicalHistoryDTO.Id });

            medicalHistoryDTO.Invoices = _mapper.Map<List<GetMedicalHistoryInvoiceDTO>>(getAllInvoicesByMedicalHistoryIdContract.Data);
            medicalHistoryDTO.MedicalOrders = _mapper.ConvertMedicalOrderContractsToDTOs(getAllMedicalOrdersByMedicalHistoryIdContract.Data);

            if (medicalHistoryDTO.MedicalHistoryType == nameof(MedicalHistoryTypeEnum.Inpatient))
            {
                var bedOccupancyContract = await _messageBus.RequestAsync<
                    GetListBedOccupancyDataByMedicalHistoryIdEvent,
                    GetListBedOccupancyDataContract>(new() { MedicalHistoryId = medicalHistoryDTO.Id });

                medicalHistoryDTO.BedOccupancies = _mapper.Map<List<GetMedicalHistoryBedOccupancyDTO>>(
                    bedOccupancyContract.Data);
                medicalHistoryDTO.BedOccupancy = this.GetCurrentOrLatestBedOccupancy(
                    medicalHistoryDTO.BedOccupancies);
            }
        }
        #endregion

        #region Helper Function
        private void FilterComplaintByCurrentUserRole(List<GetMedicalHistoryDTO> medicalHistoryDTOs)
        {
            bool skipFiltering = _currentUserService.Role
                                    is RoleEnum.Manager
                                    or RoleEnum.HotlineAgent;
            if (skipFiltering)
            {
                return;
            }

            foreach (var medicalHistoryDTO in medicalHistoryDTOs)
            {
                switch (_currentUserService.Role)
                {
                    case RoleEnum.Patient:
                        if (medicalHistoryDTO.Complaint != null)
                        {
                            if (medicalHistoryDTO.Complaint.ComplaintResolveStatus
                                is nameof(ComplaintResolveStatusEnum.Draft)
                                or nameof(ComplaintResolveStatusEnum.Closed))
                            {
                                medicalHistoryDTO.Complaint.Response = null;
                                medicalHistoryDTO.Complaint.ComplaintResolveStatus = nameof(ComplaintResolveStatusEnum.Pending);
                            }

                            medicalHistoryDTO.Complaint.ComplaintType = null;
                        }
                        break;
                    default:
                        medicalHistoryDTO.Complaint = null;
                        break;
                }
            }
        }

        private async Task<Expression<Func<MedicalHistory, bool>>> ApplyPatientAndDoctorFiltersAsync(
            Expression<Func<MedicalHistory, bool>> filterExpr,
            string? patientName,
            string? doctorName)
        {
            if (!string.IsNullOrEmpty(patientName))
            {
                var getPatientIdsEvent = new GetListUserIdByNameContainsAndRoleEvent
                {
                    Name = patientName,
                    Role = nameof(RoleEnum.Patient)
                };

                var getPatientIdsContract = await _messageBus.RequestAsync<
                    GetListUserIdByNameContainsAndRoleEvent,
                    GetListUserIdByNameContainsAndRoleContract>(getPatientIdsEvent);

                var patientIds = getPatientIdsContract.Ids;

                filterExpr = filterExpr.And(mh => patientIds.Contains(mh.PatientId));
            }

            if (!string.IsNullOrEmpty(doctorName))
            {
                var getDoctorIdsEvent = new GetListUserIdByNameContainsAndRoleEvent
                {
                    Name = doctorName,
                    Role = nameof(RoleEnum.Doctor)
                };

                var getDoctorIdsContract = await _messageBus.RequestAsync<
                    GetListUserIdByNameContainsAndRoleEvent,
                    GetListUserIdByNameContainsAndRoleContract>(getDoctorIdsEvent);

                var doctorIds = getDoctorIdsContract.Ids;

                filterExpr = filterExpr.And(mh => mh.DoctorId.HasValue && doctorIds.Contains(mh.DoctorId.Value));
            }

            return filterExpr;
        }

        private GetMedicalHistoryBedOccupancyDTO? GetCurrentOrLatestBedOccupancy(
            List<GetMedicalHistoryBedOccupancyDTO> bedOccupancies)
        {
            return bedOccupancies
                .OrderByDescending(occupancy => occupancy.OccupancyStatus == ACTIVE_OCCUPANCY_STATUS)
                .ThenByDescending(occupancy => occupancy.FromDateTime)
                .FirstOrDefault();
        }
        #endregion
    }
}
