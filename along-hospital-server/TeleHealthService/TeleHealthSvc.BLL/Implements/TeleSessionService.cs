using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using System.Text.Json;
using TeleHealthSvc.BLL.DTOs.TeleSessionDTOs;
using TeleHealthSvc.BLL.Interfaces;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.BLL.Implements
{
    public class TeleSessionService(
        IUnitOfWork unitOfWork,
        IMessageBus messageBus,
        AppConfiguration configuration,
        ICurrentUserService currentUserService) : ITeleSessionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly AppConfiguration _configuration = configuration;

        private readonly IGenericRepository<TeleSession> _teleSessionRepository = unitOfWork.Repository<TeleSession>();
        private readonly IGenericRepository<TeleRoom> _teleRoomRepository = unitOfWork.Repository<TeleRoom>();

        private const string HubUrl = "/hubs/meeting";
        private const string MeetingUrl = "/patient/appointments/meeting-room-token/";

        #region Primary Methods
        public async Task CreateTeleSessionByAppointmentDataAsync(CreateTeleSessionRequestDTO createTeleSessionRequestDTO)
        {
            var teleRoom = await _teleRoomRepository
                .GetByConditionAsync(tr => tr.SpecialtyId == createTeleSessionRequestDTO.SpecialtyId)
                    ?? throw new DataNotFoundException(typeof(TeleRoom), createTeleSessionRequestDTO.SpecialtyId);

            var credentialMetadata = new TeleSessionCredentialDTO
            {
                IceServers = _configuration.WebRtcConfig.IceServers.Select(s => new IceServerDTO
                {
                    Urls = s.Urls,
                    Username = s.Username,
                    Credential = s.Credential
                }),
                SignalR = new SignalRMetadataDTO
                {
                    HubUrl = $"{_configuration.UrlsConfig.BackendUrl}{HubUrl}"
                }
            };
            var credentialJson = JsonSerializer.Serialize(credentialMetadata);

            var teleSession = new TeleSession
            {
                AppointmentId = createTeleSessionRequestDTO.AppointmentId,
                TeleRoomId = teleRoom.Id,
                PatientId = createTeleSessionRequestDTO.PatientId,
                CredentialMetadataJson = credentialJson,
                Date = createTeleSessionRequestDTO.Date,
                StartTime = createTeleSessionRequestDTO.StartTime,
                EndTime = createTeleSessionRequestDTO.EndTime
            };

            await _teleSessionRepository.AddAsync(teleSession);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<TeleSessionCredentialDTO> GetTeleSessionByTransactionIdAsync(Guid transactionId)
        {
            var appointmentContract = await _messageBus.RequestAsync<GetAppointmentByTransactionIdEvent, GetAppointmentContract>(
                new GetAppointmentByTransactionIdEvent
                {
                    TransactionId = transactionId
                });

            var teleSession = await _teleSessionRepository
                .GetByConditionAsync(ts => ts.AppointmentId == appointmentContract.Id)
                    ?? throw new DataNotFoundException("Cannot find tele session.");

            var teleRoom = await _teleRoomRepository
                .GetByIdAsync(teleSession.TeleRoomId)
                    ?? throw new DataNotFoundException(typeof(TeleRoom), teleSession.TeleRoomId);

            this.ValidatePatient(_currentUserService.UserId, teleSession, appointmentContract);

            TeleSessionCredentialDTO credentialMetadata = JsonSerializer
                .Deserialize<TeleSessionCredentialDTO>(teleSession.CredentialMetadataJson)
                    ?? throw new DataNotFoundException("Cannot find tele session credential metadata.");

            var expireAt = teleSession.Date.ToDateTime(teleSession.EndTime);
            var serverNow = DateTime.UtcNow.ConvertTimeToTimeZone();

            return new TeleSessionCredentialDTO
            {
                IceServers = credentialMetadata.IceServers,
                SignalR = credentialMetadata.SignalR,
                RoomDisplayName = teleRoom.RoomDisplayName,
                ExpireAt = expireAt,
                ServerNow = serverNow,
            };
        }

        public async Task<PatientJoinSessionInfoDTO> GetPatientJoinInfoByTransactionIdAsync(Guid transactionId)
        {
            var appointmentContract = await _messageBus.RequestAsync<GetAppointmentByTransactionIdEvent, GetAppointmentContract>(
                new GetAppointmentByTransactionIdEvent
                {
                    TransactionId = transactionId
                });

            var teleSession = await _teleSessionRepository
                .GetByConditionAsync(ts => ts.AppointmentId == appointmentContract.Id)
                    ?? throw new DataNotFoundException("Tele session was not found.");

            var teleRoom = await _teleRoomRepository
                .GetByIdAsync(teleSession.TeleRoomId)
                    ?? throw new DataNotFoundException(typeof(TeleRoom), teleSession.TeleRoomId);

            this.ValidatePatient(_currentUserService.UserId, teleSession, appointmentContract);

            var expireAt = teleSession.Date.ToDateTime(teleSession.EndTime);
            var serverNow = DateTime.UtcNow.ConvertTimeToTimeZone();

            return new PatientJoinSessionInfoDTO
            {
                RoomCode = teleRoom.RoomCode,
                ExpireAt = expireAt,
                ServerNow = serverNow
            };
        }

        public async Task<List<GetTeleSessionDTO>> GetTeleSessionByListAppointmentIdAsync(List<GetTeleSessionByAppointmentIdEventItem> appointments)
        {
            var appointmentIds = appointments.Select(x => x.AppointmentId).Distinct().ToList();
            var teleSessions = await _teleSessionRepository.GetAllAsync(ts => appointmentIds.Contains(ts.AppointmentId));
            var transactionLookup = appointments
                .GroupBy(x => x.AppointmentId)
                .ToDictionary(x => x.Key, x => x.First().TransactionId);

            return teleSessions.Select(ts => new GetTeleSessionDTO
            {
                AppointmentId = ts.AppointmentId,
                Date = ts.Date,
                StartTime = ts.StartTime,
                EndTime = ts.EndTime,
                PatientJoinUrl = transactionLookup.TryGetValue(ts.AppointmentId, out var transactionId) && transactionId.HasValue
                    ? $"{_configuration.UrlsConfig.FrontendUrl}{MeetingUrl}{transactionId.Value}"
                    : null
            }).ToList();
        }
        #endregion

        #region Helper Methods
        private void ValidatePatient(int userId, TeleSession teleSession, GetAppointmentContract appointmentContract)
        {
            var now = DateTime.UtcNow.ConvertTimeToTimeZone();
            var start = teleSession.Date.ToDateTime(teleSession.StartTime);
            var end = teleSession.Date.ToDateTime(teleSession.EndTime);

            if (teleSession.PatientId != userId)
            {
                throw new UnauthorizedAccessException("User is not authorized to access this tele session.");
            }

            if (appointmentContract.AppointmentPaymentStatus != "Completed")
            {
                throw new UnauthorizedAccessException("Appointment payment is not completed.");
            }

            if (now < start)
            {
                throw new UnauthorizedAccessException("Tele session has not started yet.");
            }

            if (now >= end)
            {
                throw new UnauthorizedAccessException("Tele session has expired.");
            }
        }
        #endregion
    }
}