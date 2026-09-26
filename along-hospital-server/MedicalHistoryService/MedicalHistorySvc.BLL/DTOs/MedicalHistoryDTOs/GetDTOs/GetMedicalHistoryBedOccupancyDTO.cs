namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs
{
    public class GetMedicalHistoryBedOccupancyDTO
    {
        public int Id { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public double DurationInDays { get; set; }
        public double UnitPrice { get; set; }
        public double TotalAmount { get; set; }
        public string? OccupancyStatus { get; set; }
        public string? TransferNote { get; set; }
        public string? LatestTransferNote { get; set; }
        public int MedicalHistoryId { get; set; }
        public int BedId { get; set; }
        public GetMedicalHistoryBedDTO? Bed { get; set; }
    }

    public class GetMedicalHistoryBedDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int BedCategoryId { get; set; }
        public string? BedCategoryCode { get; set; }
        public string? BedCategoryName { get; set; }
        public GetMedicalHistoryRoomDTO? Room { get; set; }
    }

    public class GetMedicalHistoryRoomDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? BuildingName { get; set; }
        public int FloorNumber { get; set; }
        public int SpecialtyId { get; set; }
        public string? SpecialtyName { get; set; }
    }
}
