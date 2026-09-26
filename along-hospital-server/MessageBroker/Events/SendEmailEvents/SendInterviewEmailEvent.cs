using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendInterviewEmailEvent : BaseEvent
    {
        public string? CandidateName { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public DateOnly ApplyDate { get; init; }
        public string? JobTitle { get; init; }
        public DateTime InterviewDate { get; init; }
        public string? Note { get; init; }
        public string? InterviewTypeName { get; init; }
        public string? InterviewTypeDescription { get; init; }
    }
}