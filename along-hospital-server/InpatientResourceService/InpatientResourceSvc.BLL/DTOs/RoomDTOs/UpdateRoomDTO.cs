using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.RoomDTOs
{
    public class UpdateRoomDTO : MapTo<Room>
    {
        public string? Status { get; set; }

        public int FloorId { get; set; }

        public int RoomCategoryId { get; set; }

        public int SpecialtyId { get; set; }
    }
}
