using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace InpatientResourceSvc.BLL.DTOs.RoomDTOs
{
    public class CreateRoomDTO : MapTo<Room>
    {
        [JsonIgnore]
        public string? Code { get; set; }

        public int FloorId { get; set; }

        public int SpecialtyId { get; set; }

        public int RoomCategoryId { get; set; }
    }
}
