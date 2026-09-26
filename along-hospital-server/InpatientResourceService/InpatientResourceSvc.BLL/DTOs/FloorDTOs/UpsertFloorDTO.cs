using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.FloorDTOs
{
    public class UpsertFloorDTO : MapTo<Floor>
    {
        public int FloorNumber { get; set; }
        public int BuildingId { get; set; }
    }
}