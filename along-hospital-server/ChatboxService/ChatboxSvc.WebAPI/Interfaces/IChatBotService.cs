using Microsoft.Extensions.AI;

namespace ChatboxSvc.WebAPI.Interfaces
{
    public interface IChatBotService
    {
        Task<string> GetBotResponseAsync(string userMessage, List<ChatMessage> chatHistory);
    }
}
