using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs
{
    public class AssignBedDTO : MapTo<BedOccupancy>
    {
        public int MedicalHistoryId { get; set; }

        public int BedId { get; set; }
    }
}
