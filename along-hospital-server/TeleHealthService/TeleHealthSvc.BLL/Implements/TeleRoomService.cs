using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;
using TeleHealthSvc.BLL.DTOs.TeleRoomDTOs;
using TeleHealthSvc.BLL.DTOs.TeleSessionDTOs;
using TeleHealthSvc.BLL.Interfaces;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.BLL.Implements
{
    public class TeleRoomService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        AppConfiguration appConfiguration,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : BaseService<TeleRoom, CreateTeleRoomDTO, UpdateTeleRoomDTO, GetTeleRoomDTO>(
            unitOfWork,
            mapper),
        ITeleRoomService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly AppConfiguration _appConfiguration = appConfiguration;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IGenericRepository<TeleRoom> _teleRoomRepository = unitOfWork.Repository<TeleRoom>();

        private const string HubUrl = "/hubs/meeting";

        #region Override Methods
        public override async Task<GetTeleRoomDTO> CreateAsync(CreateTeleRoomDTO createDTO)
        {
            var teleRoom = _mapper.Map<TeleRoom>(createDTO);
            await this.ValidateUniqueTeleRoomAsync(teleRoom, null);
            return await base.CreateAsync(createDTO);
        }

        public override async Task<GetTeleRoomDTO> UpdateAsync(int id, UpdateTeleRoomDTO updateDTO)
        {
            var teleRoom = _mapper.Map<TeleRoom>(updateDTO);
            await this.ValidateUniqueTeleRoomAsync(teleRoom, id);
            return await base.UpdateAsync(id, updateDTO);
        }
        #endregion

        #region Primary Methods
        public async Task<GetTeleRoomWithCredentialsDTO> GetTeleRoomForDoctorAsync()
        {
            var doctorId = _currentUserService.UserId;

            var getSpecialtyIdByStaffIdContract = await _messageBus
                .RequestAsync<GetStaffDataByUserIdEvent, GetStaffDataByUserIdContract>(
                new GetStaffDataByUserIdEvent
                {
                    UserId = doctorId
                });

            var teleRoom = await _teleRoomRepository
                .GetByConditionAsync(r => r.SpecialtyId == getSpecialtyIdByStaffIdContract.SpecialtyId)
                    ?? throw new DataNotFoundException(typeof(TeleRoom), getSpecialtyIdByStaffIdContract.SpecialtyId);

            var getTeleRoomWithCredentialsDTO = _mapper.Map<GetTeleRoomWithCredentialsDTO>(teleRoom);

            getTeleRoomWithCredentialsDTO.Credentials = new TeleRoomCredentialDTO
            {
                IceServers = [.. _appConfiguration.WebRtcConfig.IceServers.Select(s => new IceServerDTO
                {
                    Urls = s.Urls,
                    Username = s.Username,
                    Credential = s.Credential
                })],

                SignalR = new SignalRMetadataDTO
                {
                    HubUrl = $"{_appConfiguration.UrlsConfig.BackendUrl}{HubUrl}"
                }
            };

            return getTeleRoomWithCredentialsDTO;
        }
        #endregion

        #region Helper Methods
        private async Task ValidateUniqueTeleRoomAsync(TeleRoom teleRoom, int? currentId)
        {
            var existingBySpecialty = await _teleRoomRepository
                .GetByConditionAsync(tr => tr.SpecialtyId == teleRoom.SpecialtyId);
            if (existingBySpecialty != null && existingBySpecialty.Id != currentId)
            {
                throw new DataConflictException(typeof(TeleRoom), $"A tele room for specialty ID {teleRoom.SpecialtyId} already exists.");
            }

            var roomCode = teleRoom.RoomCode?.Trim();
            if (!string.IsNullOrWhiteSpace(roomCode))
            {
                var existingByRoomCode = await _teleRoomRepository
                    .GetByConditionAsync(tr => tr.RoomCode == roomCode);
                if (existingByRoomCode != null && existingByRoomCode.Id != currentId)
                {
                    throw new DataConflictException(typeof(TeleRoom), $"A tele room with room code '{roomCode}' already exists.");
                }
            }

            var roomDisplayName = teleRoom.RoomDisplayName?.Trim();
            if (!string.IsNullOrWhiteSpace(roomDisplayName))
            {
                var existingByRoomDisplayName = await _teleRoomRepository
                    .GetByConditionAsync(tr => tr.RoomDisplayName == roomDisplayName);
                if (existingByRoomDisplayName != null && existingByRoomDisplayName.Id != currentId)
                {
                    throw new DataConflictException(typeof(TeleRoom), $"A tele room with room display name '{roomDisplayName}' already exists.");
                }
            }
        }
        #endregion
    }
}
