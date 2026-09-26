using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;
using System.Collections.Concurrent;
using TeleHealthSvc.BLL.Interfaces;

namespace TeleHealthSvc.WebAPI.SignalRHubs
{
    [Authorize(Roles = RolePolicies.TeleHealthSessionCallRolePolicy)]
    public class MeetingHub(
        IHubContext<MeetingHub> hubContext,
        ITeleSessionService teleSessionService,
        ICurrentUserService currentUserService) : Hub
    {
        private sealed record ParticipantInfo(string Role);

        private static readonly TimeSpan ExpiringSoonThreshold = TimeSpan.FromMinutes(5);

        private readonly ITeleSessionService _teleSessionService = teleSessionService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IHubContext<MeetingHub> _hubContext = hubContext;

        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ParticipantInfo>> RoomParticipants = new();
        private static readonly ConcurrentDictionary<string, List<CancellationTokenSource>> ConnectionTimers = new();
        private static readonly ConcurrentDictionary<string, string> ActiveConnectionRooms = new();

        private string ConnId => Context.ConnectionId;

        public async Task JoinSession(string transactionId, string roomCode)
        {
            string finalRoomCode;

            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                if (!Guid.TryParse(transactionId, out var txIdGuid))
                {
                    await NotifyCaller("JoinFailed", new { message = "INVALID_TRANSACTION_ID" });
                    Context.Abort();
                    return;
                }

                var patientJoinInfo = await _teleSessionService.GetPatientJoinInfoByTransactionIdAsync(txIdGuid);
                Context.Items["ExpiresAt"] = patientJoinInfo.ExpireAt;

                finalRoomCode = patientJoinInfo.RoomCode!;
                if (string.IsNullOrWhiteSpace(finalRoomCode))
                {
                    await NotifyCaller("JoinFailed", new { message = "ROOM_NOT_FOUND" });
                    Context.Abort();
                    return;
                }
            }
            else if (!string.IsNullOrWhiteSpace(roomCode))
            {
                if (_currentUserService.Role != RoleEnum.Doctor)
                {
                    await NotifyCaller("JoinFailed", new { message = "FORBIDDEN_ROOM_ACCESS" });
                    Context.Abort();
                    return;
                }

                finalRoomCode = roomCode;
            }
            else
            {
                await NotifyCaller("JoinFailed", new { message = "NO_ROOM" });
                Context.Abort();
                return;
            }

            Context.Items["RoomCode"] = finalRoomCode;
            ActiveConnectionRooms[ConnId] = finalRoomCode;

            var currentRole = _currentUserService.Role.ToString();
            var participants = RoomParticipants.GetOrAdd(finalRoomCode, _ => new());

            if (participants.Count >= 2)
            {
                await NotifyCaller("JoinFailed", new { message = "ROOM_FULL" });
                Context.Abort();
                return;
            }

            var existingParticipants = participants
                .Where(x => x.Key != ConnId)
                .Select(x => new
                {
                    connectionId = x.Key,
                    role = x.Value.Role
                })
                .ToArray();

            participants[ConnId] = new ParticipantInfo(currentRole);

            await Groups.AddToGroupAsync(ConnId, finalRoomCode);

            await NotifyCaller("JoinSucceeded", new
            {
                roomCode = finalRoomCode,
                connectionId = ConnId,
                role = currentRole,
                existingParticipants,
                shouldCreateOffer = _currentUserService.Role == RoleEnum.Patient && existingParticipants.Length > 0
            });

            if (Context.Items.TryGetValue("ExpiresAt", out var expiresAtObj) && expiresAtObj is DateTime expiresAt)
            {
                ScheduleSessionLifecycleNotifications(ConnId, expiresAt);
            }

            await Clients.OthersInGroup(finalRoomCode).SendAsync("ParticipantJoined", new
            {
                connectionId = ConnId,
                role = currentRole
            });
        }

        public async Task LeaveSession()
        {
            if (ActiveConnectionRooms.TryGetValue(ConnId, out var room))
            {
                await RemoveConnectionFromRoomAsync(ConnId, room, notifyOthers: true);
                CancelScheduledNotifications(ConnId);

                await NotifyCaller("LeaveSucceeded", new
                {
                    message = "Left the session successfully",
                    connectionId = ConnId,
                    roomCode = room
                });
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (ActiveConnectionRooms.TryGetValue(ConnId, out var room))
            {
                CancelScheduledNotifications(ConnId);
                await RemoveConnectionFromRoomAsync(ConnId, room, notifyOthers: true);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string room, string message)
        {
            if (await GuardConnectionAsync(room))
            {
                return;
            }

            await NotifyOthers(room, "ReceiveMessage", new { from = ConnId, message });
        }

        public async Task SendOffer(string room, object offer)
        {
            if (await GuardConnectionAsync(room))
            {
                return;
            }

            await NotifyOthers(room, "ReceiveOffer", new { from = ConnId, offer });
        }

        public async Task SendAnswer(string room, object answer)
        {
            if (await GuardConnectionAsync(room))
            {
                return;
            }

            await NotifyOthers(room, "ReceiveAnswer", new { from = ConnId, answer });
        }

        public async Task SendIceCandidate(string room, object candidate)
        {
            if (await GuardConnectionAsync(room))
            {
                return;
            }

            await NotifyOthers(room, "ReceiveIceCandidate", new { from = ConnId, candidate });
        }

        public async Task NotifyState(string room, object state)
        {
            if (await GuardConnectionAsync(room))
            {
                return;
            }

            await NotifyOthers(room, "StateUpdated", new { from = ConnId, state });
        }

        private Task NotifyOthers(string room, string evt, object payload)
            => Clients.OthersInGroup(room).SendAsync(evt, payload);

        private Task NotifyCaller(string evt, object payload)
            => Clients.Caller.SendAsync(evt, payload);

        private bool IsConnectionExpired()
        {
            if (!Context.Items.TryGetValue("ExpiresAt", out var expiresAtObj))
            {
                return false;
            }

            var expiresAt = (DateTime)expiresAtObj!;
            return DateTime.UtcNow.ConvertTimeToTimeZone() >= expiresAt;
        }

        private async Task<bool> GuardConnectionAsync(string room)
        {
            if (IsConnectionExpired())
            {
                if (ActiveConnectionRooms.TryGetValue(ConnId, out var activeRoom))
                {
                    await RemoveConnectionFromRoomAsync(ConnId, activeRoom, notifyOthers: true);
                }

                await NotifyCaller("SessionExpired", new { message = "SESSION_EXPIRED" });
                Context.Abort();
                return true;
            }

            if (!ActiveConnectionRooms.TryGetValue(ConnId, out var ownedRoom) || ownedRoom != room)
            {
                await NotifyCaller("JoinFailed", new { message = "ROOM_MEMBERSHIP_REQUIRED" });
                Context.Abort();
                return true;
            }

            return false;
        }

        private void CleanupRoomIfEmpty(string room, ConcurrentDictionary<string, ParticipantInfo> participants)
        {
            if (participants.IsEmpty)
            {
                RoomParticipants.TryRemove(room, out _);
            }
        }

        private void ScheduleSessionLifecycleNotifications(string connectionId, DateTime expiresAt)
        {
            CancelScheduledNotifications(connectionId);

            var now = DateTime.UtcNow.ConvertTimeToTimeZone();
            var remaining = expiresAt - now;
            if (remaining <= TimeSpan.Zero)
            {
                return;
            }

            var timers = new List<CancellationTokenSource>();

            if (remaining <= ExpiringSoonThreshold)
            {
                var immediateWarningCts = new CancellationTokenSource();
                timers.Add(immediateWarningCts);
                _ = NotifySessionExpiringAsync(connectionId, expiresAt, immediateWarningCts.Token);
            }
            else
            {
                var warningCts = new CancellationTokenSource();
                timers.Add(warningCts);
                _ = ScheduleSessionExpiringAsync(connectionId, expiresAt, remaining - ExpiringSoonThreshold, warningCts.Token);
            }

            var expiredCts = new CancellationTokenSource();
            timers.Add(expiredCts);
            _ = ScheduleSessionExpiredAsync(connectionId, expiresAt, remaining, expiredCts.Token);

            ConnectionTimers[connectionId] = timers;
        }

        private void CancelScheduledNotifications(string connectionId)
        {
            if (!ConnectionTimers.TryRemove(connectionId, out var timers))
            {
                return;
            }

            foreach (var timer in timers)
            {
                timer.Cancel();
                timer.Dispose();
            }
        }

        private async Task ScheduleSessionExpiringAsync(
            string connectionId,
            DateTime expiresAt,
            TimeSpan delay,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(delay, cancellationToken);
                await NotifySessionExpiringAsync(connectionId, expiresAt, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private Task NotifySessionExpiringAsync(string connectionId, DateTime expiresAt, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.Client(connectionId).SendAsync("SessionExpiring", new
            {
                message = "SESSION_EXPIRING_SOON",
                expiresAt
            }, cancellationToken);
        }

        private async Task ScheduleSessionExpiredAsync(
            string connectionId,
            DateTime expiresAt,
            TimeSpan delay,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(delay, cancellationToken);

                if (ActiveConnectionRooms.TryGetValue(connectionId, out var room))
                {
                    await RemoveConnectionFromRoomAsync(connectionId, room, notifyOthers: true);
                }

                await _hubContext.Clients.Client(connectionId).SendAsync("SessionExpired", new
                {
                    message = "SESSION_EXPIRED",
                    expiresAt
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task RemoveConnectionFromRoomAsync(string connectionId, string room, bool notifyOthers)
        {
            ActiveConnectionRooms.TryRemove(connectionId, out _);

            if (RoomParticipants.TryGetValue(room, out var participants))
            {
                participants.TryRemove(connectionId, out _);
                CleanupRoomIfEmpty(room, participants);
            }

            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, room);

            if (notifyOthers)
            {
                await _hubContext.Clients.GroupExcept(room, new[] { connectionId }).SendAsync("ParticipantLeft", new
                {
                    connectionId
                });
            }
        }
    }
}