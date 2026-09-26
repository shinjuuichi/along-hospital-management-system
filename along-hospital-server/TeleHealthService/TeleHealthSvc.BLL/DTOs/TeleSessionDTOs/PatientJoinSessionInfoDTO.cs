namespace TeleHealthSvc.BLL.DTOs.TeleSessionDTOs
{
    public class PatientJoinSessionInfoDTO
    {
        public string? RoomCode { get; set; }

        public DateTime ExpireAt { get; set; }

        public DateTime ServerNow { get; set; }
    }
}
