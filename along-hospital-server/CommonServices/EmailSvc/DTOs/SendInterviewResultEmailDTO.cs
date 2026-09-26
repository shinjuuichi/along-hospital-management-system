namespace EmailSvc.DTOs
{
    public class SendInterviewResultEmailDTO
    {
        public string? CandidateName { get; set; }
        public string? Email { get; set; }
        public DateOnly ApplyDate { get; set; }
        public string? JobTitle { get; set; }
        public DateTime InterviewDate { get; set; }
        public string? Result { get; set; }
        public string? ApplicationStatus { get; set; }
    }
}
