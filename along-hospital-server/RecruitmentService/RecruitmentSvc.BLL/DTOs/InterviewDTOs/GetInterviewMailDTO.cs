namespace RecruitmentSvc.BLL.DTOs.InterviewDTOs
{
    public class GetInterviewMailDTO
    {
        public string? CandidateName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly ApplyDate { get; set; }
        public string? JobTitle { get; set; }
        public DateTime InterviewDate { get; set; }
        public string? Note { get; set; }
        public string? InterviewTypeName { get; set; }
        public string? InterviewTypeDescription { get; set; }
        public string? Result { get; set; }
        public string? ApplicationStatus { get; set; }
    }
}