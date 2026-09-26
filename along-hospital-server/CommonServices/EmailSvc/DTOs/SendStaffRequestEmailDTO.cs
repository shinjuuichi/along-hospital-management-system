namespace EmailSvc.DTOs;

public class SendStaffRequestEmailDTO
{
    public List<string> RecipientEmails { get; set; } = [];
    public string StaffName { get; set; } = string.Empty;
    public string? DeciderName { get; set; }
    public string RequestType { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? Reason { get; set; }
}
