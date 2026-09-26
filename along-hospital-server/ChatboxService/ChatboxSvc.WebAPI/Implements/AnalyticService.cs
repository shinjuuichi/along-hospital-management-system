using ChatboxSvc.WebAPI.Commons;
using ChatboxSvc.WebAPI.Interfaces;
using Microsoft.Extensions.AI;
using System.Text;

namespace ChatboxSvc.WebAPI.Implements
{
    public class AnalyticService(IChatClient chatClient) : IAnalyticService
    {
        private readonly IChatClient _chatClient = chatClient;

        private const string FallbackWeeklyComplaintSummary = "Overview\n" +
            "- Total items: 0\n" +
            "- Sentiment distribution: unclear\n" +
            "- Overall mood: unclear\n\n" +
            "One-line executive summary\n" +
            "- Unable to generate summary at this time.";

        public async Task<string> SummarizeWeeklyComplaintsAsync(List<string> complaints)
        {
            if (!complaints.Any())
            {
                return FallbackWeeklyComplaintSummary;
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < complaints.Count; i++)
            {
                var text = complaints[i]?.Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                if (text.Length > 400)
                {
                    text = text[..400];
                }

                stringBuilder.AppendLine($"{i + 1}) {text}");
            }

            var complaintText = stringBuilder.ToString();
            complaintText = complaintText.Length > 15000 ? complaintText[..15000] : complaintText;

            var analyticPrompt = $"""
                Here are patient complaints from the last 7 days.
                Analyze and summarize them according to the required output structure.

                Complaints:
                {complaintText}
                """;

            return await this.GetChatResponseAsync(GeneralLLMPrompt.WeeklySummaryComplaintPrompt, analyticPrompt, FallbackWeeklyComplaintSummary);
        }

        private async Task<string> GetChatResponseAsync(string systemPrompt, string analyticPrompt, string fallbackResponse = "")
        {
            var messages = new List<ChatMessage>
            {
                new(ChatRole.System, systemPrompt),
                new(ChatRole.User, analyticPrompt)
            };

            try
            {
                var response = await _chatClient.GetResponseAsync(messages);
                if (response == null || string.IsNullOrWhiteSpace(response.Text))
                {
                    return fallbackResponse;
                }

                var output = response.Text.Trim();
                var words = output.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 500)
                {
                    output = string.Join(" ", words.Take(500));
                }

                return output;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
