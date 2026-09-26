using SharedLibrary.Base.Mappers;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.BLL.DTOs.TeleRoomDTOs
{
    public class GetTeleRoomDTO : MapFrom<TeleRoom>
    {
        public int Id { get; set; }

        public string? RoomCode { get; set; }

        public string? RoomDisplayName { get; set; }

        public int SpecialtyId { get; set; }
    }
}