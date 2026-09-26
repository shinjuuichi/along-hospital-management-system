namespace TeleHealthSvc.BLL.DTOs.TeleSessionDTOs
{
    public class TeleSessionCredentialDTO
    {
        public IEnumerable<IceServerDTO> IceServers { get; set; } = [];

        public SignalRMetadataDTO? SignalR { get; set; }

        public string? RoomDisplayName { get; set; }

        public DateTime ExpireAt { get; set; }

        public DateTime ServerNow { get; set; }
    }

    public class IceServerDTO
    {
        public string? Urls { get; set; }

        public string? Username { get; set; }

        public string? Credential { get; set; }
    }

    public class SignalRMetadataDTO
    {
        public string? HubUrl { get; set; }
    }
}
