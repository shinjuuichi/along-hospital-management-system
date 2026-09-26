using ChatboxSvc.WebAPI.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.AI;
using System.Collections.Concurrent;

namespace ChatboxSvc.WebAPI.SignalRHubs
{
    public class ChatBotHub(IChatBotService chatBotService) : Hub
    {
        private readonly IChatBotService _chatBotService = chatBotService;
        private static readonly ConcurrentDictionary<string, List<ChatMessage>> _histories = new();

        public async Task SendUserMessage(string userMessage)
        {
            var history = _histories.GetOrAdd(Context.ConnectionId, _ => []);

            var botReply = await _chatBotService.GetBotResponseAsync(userMessage, history);

            history.Add(new ChatMessage(ChatRole.User, userMessage));
            history.Add(new ChatMessage(ChatRole.Assistant, botReply));

            await Clients.Caller.SendAsync("ReceiveBotMessage", ChatRole.Assistant, botReply);
        }

        public Task ResetConversation()
        {
            _histories.TryRemove(Context.ConnectionId, out _);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _histories.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
