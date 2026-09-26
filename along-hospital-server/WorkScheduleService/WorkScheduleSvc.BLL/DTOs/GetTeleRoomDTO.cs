namespace WorkScheduleSvc.BLL.DTOs
{
    public class GetTeleRoomDTO
    {
        public int Id { get; set; }

        public string? RoomCode { get; set; }

        public string? RoomDisplayName { get; set; }

        public int SpecialtyId { get; set; }
    }
}