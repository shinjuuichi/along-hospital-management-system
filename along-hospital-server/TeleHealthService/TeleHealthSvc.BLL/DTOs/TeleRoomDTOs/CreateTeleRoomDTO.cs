using SharedLibrary.Base.Mappers;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.BLL.DTOs.TeleRoomDTOs
{
    public class CreateTeleRoomDTO : MapTo<TeleRoom>
    {
        public string? RoomCode { get; set; }

        public string? RoomDisplayName { get; set; }

        public int SpecialtyId { get; set; }
    }
}
