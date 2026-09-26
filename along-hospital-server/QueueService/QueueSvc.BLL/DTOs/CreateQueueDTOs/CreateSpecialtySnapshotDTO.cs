using QueueSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Mappers;

namespace QueueSvc.BLL.DTOs.CreateQueueDTOs
{
    public class CreateSpecialtySnapshotDTO : MapTo<SpecialtySnapshot>
    {
        public string? Name { get; set; }
    }
}
