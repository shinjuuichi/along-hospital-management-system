using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.InPatientResourceEvents;
using MessageBroker.Events.TeleHealthEvents.TeleRoomEvents;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.Interfaces.Gateways;

namespace WorkScheduleSvc.BLL.Implements.Gateways
{
    public class WorkScheduleAssignmentGateway(
        IMessageBus messageBus,
        IMapper mapper)
        : IWorkScheduleAssignmentGateway
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IMapper _mapper = mapper;

        public async Task<GetStaffDTO> GetStaffAsync(int staffId)
        {
            var staffContract = await _messageBus.RequestAsync<GetStaffDataByUserIdEvent, GetStaffDataByUserIdContract>(
                new GetStaffDataByUserIdEvent { UserId = staffId });
            return _mapper.Map<GetStaffDTO>(staffContract);
        }

        public async Task<List<GetStaffDTO>> GetStaffsAsync(List<int> staffIds)
        {
            if (staffIds.Count == 0)
            {
                return [];
            }

            var staffContracts = await _messageBus.RequestAsync<GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>(
                new GetListStaffDataByUserIdsEvent { UserIds = staffIds });
            return _mapper.Map<List<GetStaffDTO>>(staffContracts.Data);
        }

        public async Task<GetRoomDTO> GetRoomAsync(int roomId)
        {
            var roomContract = await _messageBus.RequestAsync<GetRoomByIdEvent, GetRoomContract>(
                new GetRoomByIdEvent { Id = roomId });
            return _mapper.Map<GetRoomDTO>(roomContract);
        }

        public async Task<GetTeleRoomDTO> GetTeleRoomAsync(int teleRoomId)
        {
            var teleRoomContract = await _messageBus.RequestAsync<GetTeleRoomByIdEvent, GetTeleRoomContract>(
                new GetTeleRoomByIdEvent { Id = teleRoomId });
            return _mapper.Map<GetTeleRoomDTO>(teleRoomContract);
        }

        public async Task<List<GetStaffDTO>> GetListStaffDataByRoleAsync()
        {
            var allowedRoles = RolePolicies.QueueManagementRolePolicy.Split(',').ToList();

            var staffContracts = await _messageBus.RequestAsync<GetListUserDataByListRoleEvent, GetListUserDataByListRoleContract>(
                new GetListUserDataByListRoleEvent { Roles = allowedRoles });
            return _mapper.Map<List<GetStaffDTO>>(staffContracts.Data);
        }

        public async Task<List<GetStaffDTO>> GetDoctorStaffDataAsync(int? specialtyId = null)
        {
            var doctorUsersContract = await _messageBus.RequestAsync<GetListUserDataByListRoleEvent, GetListUserDataByListRoleContract>(
                new GetListUserDataByListRoleEvent { Roles = [nameof(RoleEnum.Doctor)] });

            var doctorUserIds = doctorUsersContract.Data
                .Select(x => x.UserId)
                .Distinct()
                .ToList();

            if (doctorUserIds.Count == 0)
            {
                return [];
            }

            var doctorStaffContract = await _messageBus.RequestAsync<GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>(
                new GetListStaffDataByUserIdsEvent { UserIds = doctorUserIds });

            var doctors = _mapper.Map<List<GetStaffDTO>>(doctorStaffContract.Data);
            if (!specialtyId.HasValue)
            {
                return doctors;
            }

            return doctors
                .Where(x => x.SpecialtyId == specialtyId.Value)
                .ToList();
        }
    }
}
