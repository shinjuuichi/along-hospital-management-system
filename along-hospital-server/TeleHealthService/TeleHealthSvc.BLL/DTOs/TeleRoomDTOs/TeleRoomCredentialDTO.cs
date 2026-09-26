using TeleHealthSvc.BLL.DTOs.TeleSessionDTOs;

namespace TeleHealthSvc.BLL.DTOs.TeleRoomDTOs
{
    public class TeleRoomCredentialDTO
    {
        public IEnumerable<IceServerDTO> IceServers { get; set; } = [];

        public SignalRMetadataDTO? SignalR { get; set; } = new();
    }
}