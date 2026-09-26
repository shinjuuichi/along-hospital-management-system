namespace WorkScheduleSvc.BLL.DTOs
{
    public class GetRoomDTO
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Status { get; set; }

        public int BuildingId { get; set; }

        public string? BuildingName { get; set; }

        public int FloorId { get; set; }

        public int FloorNumber { get; set; }

        public int RoomCategoryId { get; set; }

        public string? RoomCategoryName { get; set; }

        public List<string> Roles { get; set; } = [];

        public int SpecialtyId { get; set; }

        public string? SpecialtyName { get; set; }
    }
}