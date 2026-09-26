using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.FloorDTOs
{
    public class GetFloorDTO : MapFrom<Floor>
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public int BuildingId { get; set; }
        public string? BuildingName { get; set; }
    }
}