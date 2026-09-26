namespace EmailSvc.DTOs
{
    public class SendContractExpiringEmailDTO
    {
        public string? Email { get; set; }

        public string? StaffName { get; set; }

        public string? Subject { get; set; }

        public DateOnly ContractEndDate { get; set; }

        public int DaysRemaining { get; set; }
    }
}
