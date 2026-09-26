using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.RoomCategoryDTOs
{
    public class UpsertRoomCategoryDTO : MapTo<RoomCategory>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<string> Roles { get; set; } = [];
    }
}
