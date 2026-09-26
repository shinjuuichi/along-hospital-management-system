using AppointmentSvc.BLL.DTOs.GetAppointmentDTOs;
using AppointmentSvc.BLL.FilterDTOs;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.MedicalServiceEvents;
using MessageBroker.Events.PaymentEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace AppointmentSvc.BLL.Implements
{
    public class AppointmentQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService) : IAppointmentQueryService
    {
        private const string DEFAULT_PAYMENT_TYPE = "PayOS";

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        private readonly IGenericRepository<Appointment> _appointmentRepository = unitOfWork.Repository<Appointment>();

        #region Get Paginated
        public async Task<PaginationResult<GetAppointmentDTO>> GetAllPaginatedAsync(AppointmentFilterDTO appointmentFilterDTO)
        {
            var (total, appointments) = await _appointmentRepository.GetAllPaginatedAsync(
                appointmentFilterDTO.Filter,
                appointmentFilterDTO.Sort,
                appointmentFilterDTO.Page,
                appointmentFilterDTO.PageSize);

            var appointmentDTOs = _mapper.Map<List<GetAppointmentDTO>>(appointments);
            await this.RequestValueForAppointmentDTOsAsync(appointmentDTOs);
            return new PaginationResult<GetAppointmentDTO>(total, appointmentFilterDTO.PageSize, appointmentDTOs);
        }

        public async Task<PaginationResult<GetAppointmentDTO>> GetAllTelehealthPaginatedByCurrentDoctorSpecialtyAsync(AppointmentFilterDTO appointmentFilterDTO)
        {
            var getStaffDataByUserIdEvent = new GetStaffDataByUserIdEvent { UserId = _currentUserService.UserId };
            var getStaffDataByUserIdContract = await _messageBus
                .RequestAsync<GetStaffDataByUserIdEvent, GetStaffDataByUserIdContract>(getStaffDataByUserIdEvent);

            appointmentFilterDTO.MeetingType = nameof(AppointmentMeetingTypeEnum.Telehealth);
            appointmentFilterDTO.SpecialtyId = getStaffDataByUserIdContract.SpecialtyId;

            var (total, appointments) = await _appointmentRepository.GetAllPaginatedAsync(
                a => a.SpecialtyId == getStaffDataByUserIdContract.SpecialtyId
                    && a.AppointmentMeetingType == AppointmentMeetingTypeEnum.Telehealth,
                appointmentFilterDTO.Filter,
                appointmentFilterDTO.Sort,
                appointmentFilterDTO.Page,
                appointmentFilterDTO.PageSize);

            var appointmentDTOs = _mapper.Map<List<GetAppointmentDTO>>(appointments);
            await this.RequestValueForAppointmentDTOsAsync(appointmentDTOs);
            return new PaginationResult<GetAppointmentDTO>(total, appointmentFilterDTO.PageSize, appointmentDTOs);
        }

        public async Task<PaginationResult<GetAppointmentDTO>> GetAllPaginatedByPatientIdAsync(int patientId, AppointmentFilterDTO appointmentFilterDTO)
        {
            var (total, appointments) = await _appointmentRepository.GetAllPaginatedAsync(
                a => a.PatientId == patientId,
                appointmentFilterDTO.Filter,
                appointmentFilterDTO.Sort,
                appointmentFilterDTO.Page,
                appointmentFilterDTO.PageSize);

            var appointmentDTOs = _mapper.Map<List<GetAppointmentDTO>>(appointments);
            await this.RequestValueForAppointmentDTOsAsync(appointmentDTOs);
            return new PaginationResult<GetAppointmentDTO>(total, appointmentFilterDTO.PageSize, appointmentDTOs);
        }
        #endregion

        #region Get All
        public async Task<List<GetAppointmentDTO>> GetAllByIdsAsync(List<int> ids, bool ignoreRequestingValue = false)
        {
            var appointments = await _appointmentRepository.GetAllAsync(a => ids.Contains(a.Id));

            var appointmentDTOs = _mapper.Map<List<GetAppointmentDTO>>(appointments);
            if (!ignoreRequestingValue)
            {
                await this.RequestValueForAppointmentDTOsAsync(appointmentDTOs);
            }

            return appointmentDTOs;
        }

        public async Task<List<GetAppointmentDTO>> GetAllByDateAsync(DateOnly date, bool ignoreRequestingValue = false)
        {
            var appointments = await _appointmentRepository.GetAllAsync(a => a.Date == date);

            var appointmentDTOs = _mapper.Map<List<GetAppointmentDTO>>(appointments);
            if (!ignoreRequestingValue)
            {
                await this.RequestValueForAppointmentDTOsAsync(appointmentDTOs);
            }

            return appointmentDTOs;
        }
        #endregion

        public async Task<GetAppointmentDTO> GetByIdAsync(int id, bool ignoreRequestingValue = false)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
            {
                throw new DataNotFoundException(typeof(Appointment), id);
            }

            var appointmentDTO = _mapper.Map<GetAppointmentDTO>(appointment);
            if (!ignoreRequestingValue)
            {
                await this.RequestValueForAppointmentDTOsAsync([appointmentDTO]);
            }

            return appointmentDTO;
        }

        public async Task<GetAppointmentDTO> GetByTransactionIdAsync(Guid transactionId)
        {
            var appointment = await _appointmentRepository.GetByConditionAsync(a => a.TransactionId == transactionId)
                ?? throw new DataNotFoundException($"{nameof(Appointment)} with TransactionId {transactionId} was not found.");
            var appointmentDTO = _mapper.Map<GetAppointmentDTO>(appointment);

            return appointmentDTO;
        }

        public async Task<string> GetPaymentUrlByAppointmentIdAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new DataNotFoundException(typeof(Appointment), appointmentId);
            }

            if (appointment.AppointmentPaymentStatus != AppointmentPaymentStatusEnum.Pending)
            {
                throw new InvalidDataException("Payment URL can only be retrieved for appointments with pending payment status");
            }

            var nowTimeZone = DateTime.UtcNow.ConvertTimeToTimeZone();
            var appointmentDateTime = appointment.Date.ToDateTime(appointment.TimeSlotSnapshot!.Time);
            if (appointmentDateTime < nowTimeZone)
            {
                throw new InvalidDataException("Cannot retrieve payment URL for past appointments");
            }

            if (!appointment.TransactionId.HasValue)
            {
                return await this.CreatePaymentUrlForAppointmentAsync(appointment);
            }

            var getPaymentUrlContract = await _messageBus.RequestAsync<
                CheckPaymentExistByTransactionIdEvent,
                CheckPaymentExistByTransactionIdContract>(new()
                {
                    TransactionId = appointment.TransactionId.Value
                });

            if (string.IsNullOrEmpty(getPaymentUrlContract.PaymentUrl))
            {
                return await this.CreatePaymentUrlForAppointmentAsync(appointment);
            }

            return getPaymentUrlContract.PaymentUrl;
        }

        #region Private Helpers
        private async Task RequestValueForAppointmentDTOsAsync(List<GetAppointmentDTO> appointmentDTOs)
        {
            // Distinct SpecialtyIds, PatientIds
            var distinctSpecialtyIds = appointmentDTOs.Select(a => a.SpecialtyId).Distinct().ToList();
            var distinctPatientIds = appointmentDTOs.Select(a => a.PatientId).Distinct().ToList();
            var distinctTelehealthAppointments = appointmentDTOs.Where(a => a.AppointmentMeetingType == AppointmentMeetingTypeEnum.Telehealth.ToString())
                .Select(a => new GetTeleSessionByAppointmentIdEventItem
                {
                    AppointmentId = a.Id,
                    TransactionId = a.TransactionId
                })
                .Distinct().ToList();

            Dictionary<int, GetSpecialtyByIdContract> specialtyContractDict = [];
            Dictionary<int, GetPatientDataByUserIdContract> patientContractDict = [];
            Dictionary<int, GetTeleSessionByAppointmentIdContract> teleSessionContractDict = [];

            // Request specialties, patients by IDs
            if (distinctSpecialtyIds.Count > 0)
            {
                var getListSpecialtyByIdsEvent = new GetListSpecialtyDataByIdsEvent { SpecialtyIds = distinctSpecialtyIds };
                var getListSpecialtyByIdsContract = await _messageBus.RequestAsync<GetListSpecialtyDataByIdsEvent, GetListSpecialtyDataByIdsContract>(getListSpecialtyByIdsEvent);
                if (getListSpecialtyByIdsContract.IsSuccess)
                {
                    specialtyContractDict = getListSpecialtyByIdsContract.Data.ToDictionary(specialty => specialty.Id);
                }
            }

            if (distinctPatientIds.Count > 0)
            {
                var getListPatientDataByIdsEvent = new GetListPatientDataByUserIdsEvent { UserIds = distinctPatientIds };
                var getListPatientDataByIdsContract = await _messageBus.RequestAsync<GetListPatientDataByUserIdsEvent, GetListPatientDataByUserIdsContract>(getListPatientDataByIdsEvent);
                if (getListPatientDataByIdsContract.IsSuccess)
                {
                    patientContractDict = getListPatientDataByIdsContract.Data.ToDictionary(patient => patient.UserId);
                }
            }

            // Wait for telehealth service ready
            if (distinctTelehealthAppointments.Count > 0)
            {
                var getListTeleSessionByAppointmentIdsEvent = new GetTeleSessionByAppointmentIdsEvent
                {
                    Appointments = distinctTelehealthAppointments
                };
                var getListTeleSessionByAppointmentIdsContract = await _messageBus.RequestAsync<
                    GetTeleSessionByAppointmentIdsEvent,
                    GetListTeleSessionByAppointmentIdsContract>(getListTeleSessionByAppointmentIdsEvent);
                if (getListTeleSessionByAppointmentIdsContract.IsSuccess)
                {
                    teleSessionContractDict = getListTeleSessionByAppointmentIdsContract.TeleSessions.ToDictionary(session => session.AppointmentId);
                }
            }

            // Map the results back to appointmentDTOs
            foreach (var appointmentDTO in appointmentDTOs)
            {
                if (specialtyContractDict.TryGetValue(appointmentDTO.SpecialtyId, out var specialtyContract))
                {
                    appointmentDTO.Specialty = _mapper.Map<GetAppointmentSpecialtyDTO>(specialtyContract);
                }

                if (patientContractDict.TryGetValue(appointmentDTO.PatientId, out var patientContract))
                {
                    appointmentDTO.Patient = _mapper.Map<GetAppointmentPatientDTO>(patientContract);
                }

                if (appointmentDTO.AppointmentMeetingType == AppointmentMeetingTypeEnum.Telehealth.ToString()
                    && teleSessionContractDict.TryGetValue(appointmentDTO.Id, out var teleSessionContract))
                {
                    appointmentDTO.TeleSession = _mapper.Map<GetAppointmentTeleSessionDTO>(teleSessionContract);
                }
            }
        }

        private async Task<string> CreatePaymentUrlForAppointmentAsync(Appointment appointment)
        {
            var medicalServiceCode = appointment.AppointmentMeetingType == AppointmentMeetingTypeEnum.Telehealth
                ? MedicalServiceCodeConstants.TELEHEALTH_APPOINTMENT_CODE
                : MedicalServiceCodeConstants.GENERAL_HEALTH_CHECK_CODE;

            // Get medical service by code
            var getMedicalServiceByCodeContract = await _messageBus.RequestAsync<
                GetMedicalServiceByCodeEvent,
                GetMedicalServiceContract>(new()
                {
                    Code = medicalServiceCode
                });

            // Create payment event
            var paymentEventItem = new PaymentEventItem
            {
                ServiceName = getMedicalServiceByCodeContract.Name,
                Quantity = 1,
                UnitPrice = getMedicalServiceByCodeContract.Price
            };

            var createPaymentEvent = new CreatePaymentEvent
            {
                Provider = DEFAULT_PAYMENT_TYPE,
                Description = $"APMConsultFee {appointment.Id}",
                PaymentEventItems = [paymentEventItem]
            };

            var createPaymentContract = await _messageBus.RequestAsync<
                CreatePaymentEvent,
                CreatePaymentContract>(createPaymentEvent);
            if (createPaymentContract == null || string.IsNullOrEmpty(createPaymentContract.PaymentUrl))
            {
                throw new InvalidDataException("Failed to create payment url");
            }

            // Update appointment with TransactionId
            appointment.TransactionId = createPaymentContract.TransactionId;

            _appointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangeAsync();

            return createPaymentContract.PaymentUrl;
        }
        #endregion
    }
}
