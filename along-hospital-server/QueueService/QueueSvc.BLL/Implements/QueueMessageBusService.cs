using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.AppointmentEvents;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.InPatientResourceEvents;
using MessageBroker.Events.MedicalHistoryEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using MessageBroker.Events.WorkScheduleEvents;
using QueueSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace QueueSvc.BLL.Implements
{
    public class QueueMessageBusService(
        IMessageBus messageBus,
        ICurrentUserService currentUserService,
        IQueueCacheService queueCacheService)
            : IQueueMessageBusService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IQueueCacheService _queueCacheService = queueCacheService;

        #region Medical History
        public async Task<GetMedicalHistoryContract> GetMedicalHistoryAndValidateStatusAsync(int medicalHistoryId, string? status = null)
        {
            var medicalHistoryContract = await _messageBus.RequestAsync<
                GetMedicalHistoryByIdEvent,
                GetMedicalHistoryContract>(new() { Id = medicalHistoryId });
            var medicalHistoryStatus = medicalHistoryContract.MedicalHistoryStatus;
            if (!string.IsNullOrEmpty(status) && status != medicalHistoryStatus)
            {
                throw new InvalidDataException("Medical history status is not valid for this operation.");
            }

            return medicalHistoryContract;
        }

        public async Task<List<GetMedicalHistoryContract>> GetAllMedicalHistoryContractsByIdsAsync(
            List<int> medicalHistoryIds)
        {
            var medicalHistoryContracts = await _messageBus.RequestAsync<
                GetListMedicalHistoryDataByIdsEvent,
                GetListMedicalHistoryDataContract>(new() { Ids = medicalHistoryIds });
            return medicalHistoryContracts.Data;
        }

        public async Task AssignDoctorToMedicalHistoryAsync(int medicalHistoryId, int doctorId)
        {
            await _messageBus.RequestAsync<AssignDoctorToMedicalHistoryEvent, AssignDoctorToMedicalHistoryContract>(new()
            {
                MedicalHistoryId = medicalHistoryId,
                DoctorId = doctorId
            });
        }
        #endregion

        #region Specialty
        public async Task<GetSpecialtyByIdContract> GetSpecialtyContractAsync(int specialtyId)
        {
            var specialtyContract = await _messageBus.RequestAsync<
                GetSpecialtyByIdEvent,
                GetSpecialtyByIdContract>(new() { SpecialtyId = specialtyId });
            return specialtyContract;
        }

        public async Task<List<GetSpecialtyByIdContract>> GetAllSpecialtyContractsByIdsAsync(List<int> specialtyIds)
        {
            var specialtyContracts = await _messageBus.RequestAsync<
                GetListSpecialtyDataByIdsEvent,
                GetListSpecialtyDataByIdsContract>(new() { SpecialtyIds = specialtyIds });
            return specialtyContracts.Data;
        }
        #endregion

        #region Appointment
        public async Task<List<GetAppointmentContract>> GetAllAppointmentContractsByDateAsync(DateOnly date)
        {
            var cachedAppointments = await _queueCacheService.GetAppointmentsCacheAsync(date);
            if (cachedAppointments != null)
            {
                return cachedAppointments;
            }

            var appointmentContracts = await _messageBus.RequestAsync<
                GetListAppointmentDataByDateEvent,
                GetListAppointmentDataContract>(new() { Date = date });

            await _queueCacheService.CreateAppointmentsCacheAsync(date, appointmentContracts.Data);
            return appointmentContracts.Data;
        }

        public async Task UpdateListAppointmentToCompletedAsync(List<int> appointmentIds)
        {
            await _messageBus.PublishAsync(new UpdateListAppointmentStatusToCompletedEvent() { Ids = appointmentIds });
        }
        #endregion

        #region Work Schedule
        public async Task<List<(int RoomId, int? DoctorId)>> GetListTodayWorkingMedicalRoomAsync()
        {
            var contract = await _messageBus.RequestAsync<
                GetListTodayWorkingMedicalRoomEvent,
                GetListTodayWorkingMedicalRoomContract>(new());

            var roomDoctorPairs = contract.Data.Select(data => (data.RoomId, data.DoctorId)).ToList();

            return roomDoctorPairs;
        }

        public async Task<int?> GetTodayWorkingDoctorByRoomIdAsync(int roomId)
        {
            var contract = await _messageBus.RequestAsync<
                GetTodayWorkingDoctorByRoomIdEvent,
                GetTodayWorkingDoctorByRoomIdContract>(new() { RoomId = roomId });

            return contract.DoctorId;
        }

        public async Task<(GetRoomContract Room, GetStaffDataByUserIdContract? Doctor)> GetRoomAndDoctorContractByRoomIdAsync(int roomId)
        {
            var doctorId = await this.GetTodayWorkingDoctorByRoomIdAsync(roomId);

            var getRoomContract = await _messageBus.RequestAsync<
                GetRoomByIdEvent,
                GetRoomContract>(new() { Id = roomId });

            GetStaffDataByUserIdContract? getDoctorContract = null;

            if (doctorId.HasValue)
            {
                getDoctorContract = await _messageBus.RequestAsync<
                    GetStaffDataByUserIdEvent,
                    GetStaffDataByUserIdContract>(new() { UserId = doctorId.Value });
            }

            return (getRoomContract, getDoctorContract);
        }

        public async Task ValidateStaffActionAsync(int? roomId = null)
        {
            var userRole = _currentUserService.Role;
            var userId = _currentUserService.UserId;

            if (!roomId.HasValue)
            {
                if (userRole != RoleEnum.Receptionist)
                {
                    throw new ForbiddenException("Only receptionist can perform this action.");
                }
            }
            else
            {
                await _messageBus.RequestAsync<
                    ValidateStaffActionEvent,
                    ValidateStaffActionContract>(new() { StaffId = userId, RoomId = roomId.Value });
            }
        }
        #endregion

        #region Get All Contracts
        public async Task<List<GetRoomContract>> GetAllRoomContractsByIdsAsync(List<int> roomIds)
        {
            var roomContracts = await _messageBus.RequestAsync<
                GetListRoomDataByIdsEvent,
                GetListRoomDataContract>(new() { Ids = roomIds });

            return roomContracts.Data;
        }

        public async Task<List<GetStaffDataByUserIdContract>> GetAllDoctorContractsByIdsAsync(List<int> doctorIds)
        {
            var doctorContracts = await _messageBus.RequestAsync<
                GetListStaffDataByUserIdsEvent,
                GetListStaffDataByUserIdsContract>(new() { UserIds = doctorIds });

            return doctorContracts.Data;
        }

        public async Task<List<GetPatientDataByUserIdContract>> GetAllPatientContractsByIdsAsync(List<int> patientIds)
        {
            var patientContracts = await _messageBus.RequestAsync<
                GetListPatientDataByUserIdsEvent,
                GetListPatientDataByUserIdsContract>(new() { UserIds = patientIds });

            return patientContracts.Data;
        }
        #endregion
    }
}