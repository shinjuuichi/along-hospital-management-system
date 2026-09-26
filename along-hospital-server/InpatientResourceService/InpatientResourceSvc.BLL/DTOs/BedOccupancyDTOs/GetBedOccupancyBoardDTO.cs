using InpatientResourceSvc.BLL.DTOs.RoomDTOs;

namespace InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs
{
    public class GetBedOccupancySummaryDTO
    {
        public int BedOccupancyId { get; set; }
        public int MedicalHistoryId { get; set; }
        public string? MedicalHistoryNumber { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? MedicalHistoryStatus { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public string? OccupancyStatus { get; set; }
    }

    public class GetBedOccupancyBoardBedDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int BedCategoryId { get; set; }
        public string? BedCategoryName { get; set; }
        public GetBedOccupancySummaryDTO? CurrentOccupancy { get; set; }
    }

    public class GetBedOccupancyBoardRoomDTO
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? BuildingName { get; set; }
        public int FloorNumber { get; set; }
        public int SpecialtyId { get; set; }
        public string? SpecialtyName { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int AvailableBeds { get; set; }
        public int MaintenanceBeds { get; set; }
        public List<GetBedOccupancyBoardBedDTO> Beds { get; set; } = [];
    }
}
