namespace TeleHealthSvc.BLL.DTOs.TeleRoomDTOs
{
    public class GetTeleRoomWithCredentialsDTO : GetTeleRoomDTO
    {
        public TeleRoomCredentialDTO? Credentials { get; set; }
    }
}