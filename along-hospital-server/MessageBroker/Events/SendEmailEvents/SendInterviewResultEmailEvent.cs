using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendInterviewResultEmailEvent : BaseEvent
    {
        public string? CandidateName { get; init; }
        public string? Email { get; init; }
        public DateOnly ApplyDate { get; init; }
        public string? JobTitle { get; init; }
        public DateTime InterviewDate { get; init; }
        public string? Result { get; init; }
        public string? ApplicationStatus { get; init; }
    }
}