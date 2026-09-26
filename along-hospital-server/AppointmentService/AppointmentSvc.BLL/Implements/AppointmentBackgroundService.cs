using AppointmentSvc.BLL.DTOs.GetAppointmentDTOs;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.BLL.StateMachines;
using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.BLL.Implements
{
    public class AppointmentBackgroundService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : IAppointmentBackgroundService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IGenericRepository<Appointment> _appointmentRepository = unitOfWork.Repository<Appointment>();

        public async Task CancelOverdueAppointmentsAsync()
        {
            var nowDateTime = DateTime.UtcNow;
            var nowDate = DateOnly.FromDateTime(nowDateTime);

            // Get all appointments that are scheduled, paid and overdue by more than 3 days
            var expiredAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.AppointmentStatus == AppointmentStatusEnum.Scheduled &&
                a.AppointmentPaymentStatus == AppointmentPaymentStatusEnum.Completed &&
                a.Date.AddDays(3) < nowDate
            );

            // Get all appointments that are scheduled, unpaid and created more than 1 day ago
            var unpaidExpiredAppointments = await _appointmentRepository.GetAllAsync(a =>
                a.AppointmentStatus == AppointmentStatusEnum.Scheduled &&
                a.AppointmentPaymentStatus == AppointmentPaymentStatusEnum.Pending &&
                a.CreationDate.AddDays(1) < nowDateTime
            );

            // Combine both lists
            var allExpiredAppointments = expiredAppointments.Concat(unpaidExpiredAppointments).ToList();
            if (!allExpiredAppointments.Any())
            {
                return;
            }

            foreach (var appointment in allExpiredAppointments)
            {
                try
                {
                    this.UpdateStatus(appointment, AppointmentStatusEnum.Cancelled);
                }
                catch
                {
                    continue;
                }
            }

            _appointmentRepository.UpdateRange(allExpiredAppointments);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task SendReminderEmailAsync()
        {
            var targetDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var appointments = await _appointmentRepository.GetAllAsync(a =>
                a.AppointmentStatus == AppointmentStatusEnum.Scheduled &&
                a.AppointmentPaymentStatus == AppointmentPaymentStatusEnum.Completed &&
                a.Date == targetDate
            );

            var appointmentDTOs = _mapper.Map<List<GetAppointmentDTO>>(appointments);
            await this.RequestSpecialtiesAndPatientsForAppointmentDTOs(appointmentDTOs);

            foreach (var appointmentDTO in appointmentDTOs)
            {
                if (appointmentDTO.Patient != null && appointmentDTO.Patient.Email != null)
                {
                    var sendReminderEmailEvent = _mapper.Map<SendAppointmentReminderEmailEvent>(appointmentDTO) with
                    {
                        Email = appointmentDTO.Patient.Email,
                    };
                    await _messageBus.PublishAsync(sendReminderEmailEvent);
                }
            }
        }

        #region Private Methods
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

        private async Task RequestSpecialtiesAndPatientsForAppointmentDTOs(List<GetAppointmentDTO> appointmentDTOs)
        {
            // Distinct SpecialtyIds, PatientIds
            var distinctSpecialtyIds = appointmentDTOs.Select(a => a.SpecialtyId).Distinct().ToList();
            var distinctPatientIds = appointmentDTOs.Select(a => a.PatientId).Distinct().ToList();

            Dictionary<int, GetSpecialtyByIdContract> specialtyContractDict = [];
            Dictionary<int, GetPatientDataByUserIdContract> patientContractDict = [];

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
            }
        }
        #endregion
    }
}
