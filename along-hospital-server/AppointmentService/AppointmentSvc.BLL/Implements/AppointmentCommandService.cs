using AppointmentSvc.BLL.DTOs;
using AppointmentSvc.BLL.DTOs.GetAppointmentDTOs;
using AppointmentSvc.BLL.DTOs.TimeSlotDTOs;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.BLL.StateMachines;
using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using AutoMapper;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.AppointmentEvents;
using MessageBroker.Events.MedicalHistoryEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace AppointmentSvc.BLL.Implements
{
    public class AppointmentCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService,
        ITimeSlotService timeSlotService,
        IAppointmentQueryService appointmentQueryService)
            : IAppointmentCommandService
    {
        private const string DEFAULT_PAYMENT_STATUS_SUCCESS = "Success";
        private const string DEFAULT_PAYMENT_STATUS_FAILED = "Failed";
        private const string DEFAULT_PAYMENT_STATUS_CANCELLED = "Cancelled";
        private const int DEFAULT_TELEHEALTH_DURATION_IN_MINUTES = 30;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ITimeSlotService _timeSlotService = timeSlotService;
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;

        private readonly IGenericRepository<Appointment> _appointmentRepository = unitOfWork.Repository<Appointment>();

        #region Create
        public async Task<CreateAppointmentResponseDTO> CreateAsync(CreateAppointmentRequestDTO createAppointmentDTO)
        {
            // Check for enum validity
            if (!Enum.TryParse<AppointmentMeetingTypeEnum>(createAppointmentDTO.AppointmentMeetingType, out _))
            {
                throw new InvalidDataException("Invalid appointment meeting type");
            }

            // Check if SpecialtyId exist
            await _messageBus.RequestAsync<CheckSpecialtyExistByIdEvent, CheckSpecialtyExistByIdContract>(new() { SpecialtyId = createAppointmentDTO.SpecialtyId });

            // Check if timeslot is exist and available
            var validateTimeSlotDTO = new ValidateTimeSlotDTO
            {
                Date = createAppointmentDTO.Date,
                TimeSlotId = createAppointmentDTO.TimeSlotId,
                SpecialtyId = createAppointmentDTO.SpecialtyId,
                AppointmentMeetingType = createAppointmentDTO.AppointmentMeetingType
            };
            var isTimeSlotAvailable = await _timeSlotService.CheckTimeSlotAvailabilityAsync(validateTimeSlotDTO);
            if (!isTimeSlotAvailable)
            {
                throw new InvalidDataException("The selected time slot is not available. Please choose a different time slot.");
            }

            // Check if appointment exist at the same day
            var existingAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.Date == createAppointmentDTO.Date &&
                a.PatientId == _currentUserService.UserId &&
                a.AppointmentStatus != AppointmentStatusEnum.Cancelled
            );
            if (existingAppointments.Any())
            {
                throw new InvalidDataException("You already have an appointment scheduled on this date.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Preparing data for creating appointment
                var timeSlot = await _timeSlotService.GetByIdAsync(createAppointmentDTO.TimeSlotId);
                var endTime = timeSlot.Time.AddMinutes(DEFAULT_TELEHEALTH_DURATION_IN_MINUTES);

                createAppointmentDTO.PatientId = _currentUserService.UserId;
                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);
                appointment.TimeSlotSnapshot = _mapper.Map<TimeSlotSnapshot>(timeSlot);

                // Create appointment
                var createdAppointment = await _appointmentRepository.AddAsync(appointment);
                await _unitOfWork.SaveChangeAsync();

                // If it's telehealth appointment, create telehealth session
                if (createdAppointment.AppointmentMeetingType == AppointmentMeetingTypeEnum.Telehealth)
                {
                    var createTeleHealthSessionRequestEvent = new CreateTeleSessionByAppointmentDataEvent
                    {
                        AppointmentId = createdAppointment.Id,
                        SpecialtyId = createdAppointment.SpecialtyId,
                        PatientId = createdAppointment.PatientId,
                        Date = createdAppointment.Date,
                        StartTime = timeSlot.Time,
                        EndTime = endTime
                    };

                    await _messageBus.RequestAsync<
                         CreateTeleSessionByAppointmentDataEvent,
                         CreateTeleSessionByAppointmentDataContract>(createTeleHealthSessionRequestEvent);
                }

                await _unitOfWork.CommitTransactionAsync();

                var paymentUrl = await _appointmentQueryService.GetPaymentUrlByAppointmentIdAsync(createdAppointment.Id);
                return new CreateAppointmentResponseDTO
                {
                    AppointmentMeetingType = createdAppointment.AppointmentMeetingType.ToString(),
                    PaymentUrl = paymentUrl
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        #endregion

        #region State Machine Handling
        public async Task UpdateStatusAsync(int appointmentId, AppointmentStatusEnum appointmentStatus)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new DataNotFoundException(typeof(Appointment), appointmentId);
            }

            var oldAppointmentStatus = appointment.AppointmentStatus;
            var oldPaymentStatus = appointment.AppointmentPaymentStatus;

            this.UpdateStatus(appointment, appointmentStatus);
            _appointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangeAsync();

            await this.PublishSyncAppointmentToQueueCacheIfNeededAsync(
                appointment,
                oldAppointmentStatus,
                oldPaymentStatus);
        }

        public async Task UpdateListStatusAsync(List<int> appointmentIds, AppointmentStatusEnum appointmentStatus)
        {
            var appointments = await _appointmentRepository.GetAllAsync(a => appointmentIds.Contains(a.Id));
            var updatedAppointment = new List<Appointment>();

            foreach (var appointment in appointments)
            {
                try
                {
                    this.UpdateStatus(appointment, appointmentStatus);
                    updatedAppointment.Add(appointment);
                }
                catch
                {
                    continue;
                }
            }

            _appointmentRepository.UpdateRange(updatedAppointment);
            await _unitOfWork.SaveChangeAsync();
        }

        private void UpdateStatus(Appointment appointment, AppointmentStatusEnum appointmentStatus)
        {
            var appointmentStateMachine = new AppointmentStatusStateMachine(appointment);
            if (!appointmentStateMachine.CanFire(appointmentStatus))
            {
                throw new InvalidDataException($"Cannot change appointment status from {appointment.AppointmentStatus} to {appointmentStatus}");
            }

            try
            {
                appointmentStateMachine.Fire(appointmentStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change appointment status: {e.Message}");
            }
        }

        private void UpdatePaymentStatus(Appointment appointment, AppointmentPaymentStatusEnum appointmentPaymentStatus)
        {
            var appointmentStateMachine = new AppointmentPaymentStatusStateMachine(appointment);
            if (!appointmentStateMachine.CanFire(appointmentPaymentStatus))
            {
                throw new InvalidDataException($"Cannot change appointment payment status from {appointment.AppointmentPaymentStatus} to {appointmentPaymentStatus}");
            }

            try
            {
                appointmentStateMachine.Fire(appointmentPaymentStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change appointment payment status: {e.Message}");
            }
        }
        #endregion

        public async Task HandlePaymentStatusChangedAsync(PaymentStatusChangedDTO paymentStatusChangedDTO)
        {
            var appointment = await _appointmentRepository.GetByConditionAsync(
                a => a.TransactionId == paymentStatusChangedDTO.TransactionId)
                ?? throw new DataNotFoundException($"{nameof(Appointment)} with TransactionId {paymentStatusChangedDTO.TransactionId} was not found!");

            var oldAppointmentStatus = appointment.AppointmentStatus;
            var oldPaymentStatus = appointment.AppointmentPaymentStatus;

            switch (paymentStatusChangedDTO.PaymentStatus)
            {
                case DEFAULT_PAYMENT_STATUS_SUCCESS:
                    this.UpdatePaymentStatus(appointment, AppointmentPaymentStatusEnum.Completed);

                    var createMedicalHistoryContract = await _messageBus.RequestAsync<
                        CreateMedicalHistoryFromAppointmentEvent,
                        CreateMedicalHistoryFromAppointmentContract>(new()
                        {
                            PatientId = appointment.PatientId,
                            SpecialtyId = appointment.SpecialtyId,
                        });

                    appointment.MedicalHistoryId = createMedicalHistoryContract.MedicalHistoryId;
                    break;

                case DEFAULT_PAYMENT_STATUS_FAILED:
                case DEFAULT_PAYMENT_STATUS_CANCELLED:
                    appointment.TransactionId = null;
                    break;

                default:
                    return;
            }

            _appointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangeAsync();

            await this.PublishSyncAppointmentToQueueCacheIfNeededAsync(
                appointment,
                oldAppointmentStatus,
                oldPaymentStatus);
        }

        private async Task PublishSyncAppointmentToQueueCacheIfNeededAsync(
            Appointment appointment,
            AppointmentStatusEnum oldAppointmentStatus,
            AppointmentPaymentStatusEnum oldPaymentStatus)
        {
            var nowTimeZone = DateTime.UtcNow.ConvertTimeToTimeZone();
            var isAppointmentToday = appointment.Date == DateOnly.FromDateTime(nowTimeZone);

            var isPaymentCompletedToday = oldPaymentStatus != AppointmentPaymentStatusEnum.Completed
                && appointment.AppointmentPaymentStatus == AppointmentPaymentStatusEnum.Completed;
            var isCancelledAfterPaidToday = oldAppointmentStatus != AppointmentStatusEnum.Cancelled
                && appointment.AppointmentStatus == AppointmentStatusEnum.Cancelled
                && appointment.AppointmentPaymentStatus == AppointmentPaymentStatusEnum.Completed;

            var shouldSyncQueueCache = isAppointmentToday
                && (isPaymentCompletedToday || isCancelledAfterPaidToday);

            if (!shouldSyncQueueCache)
            {
                return;
            }

            var appointmentDTO = _mapper.Map<GetAppointmentDTO>(appointment);
            var appointmentCacheContract = _mapper.Map<GetAppointmentContract>(appointmentDTO);

            await _messageBus.PublishAsync(new SyncAppointmentToQueueCacheEvent
            {
                Appointment = appointmentCacheContract
            });
        }
    }
}
