using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using System.Text.Json.Serialization;

namespace InpatientResourceSvc.BLL.DTOs.BedDTOs
{
    public class UpsertBedDTO : MapTo<Bed>
    {
        [JsonIgnore]
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int RoomId { get; set; }
        public int BedCategoryId { get; set; }
    }
}