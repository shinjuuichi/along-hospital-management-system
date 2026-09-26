using SharedLibrary.Commons.Filters;

namespace TeleHealthSvc.BLL.FilterDTOs
{
    public class TeleRoomFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? RoomCode { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? RoomDisplayName { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? SpecialtyId { get; set; }
    }
}