using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.GetQueueDTOs
{
    public class GetSpecialtySnapshotDTO : MapFrom<SpecialtySnapshot>
    {
        public string? Name { get; set; }
    }
}