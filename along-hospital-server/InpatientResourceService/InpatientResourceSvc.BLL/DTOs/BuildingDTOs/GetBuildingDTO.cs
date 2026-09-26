using InpatientResourceSvc.BLL.DTOs.FloorDTOs;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BuildingDTOs
{
    public class GetBuildingDTO : MapFrom<Building>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public List<GetFloorDTO> Floors { get; set; } = [];
    }
}