using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BuildingDTOs
{
    public class UpsertBuildingDTO : MapTo<Building>
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
    }
}
