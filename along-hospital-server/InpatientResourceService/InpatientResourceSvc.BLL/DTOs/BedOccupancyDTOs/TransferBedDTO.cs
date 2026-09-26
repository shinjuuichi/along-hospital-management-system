using SharedLibrary.Commons.EntityAnnotations;

namespace InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs
{
    public class TransferBedDTO
    {
        public int MedicalHistoryId { get; set; }

        public int BedId { get; set; }

        [MessageRequired]
        public string TransferNote { get; set; } = string.Empty;
    }
}
