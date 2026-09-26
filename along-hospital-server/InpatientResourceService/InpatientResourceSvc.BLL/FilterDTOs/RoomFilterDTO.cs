using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Commons.Filters;

namespace InpatientResourceSvc.BLL.FilterDTOs
{
    public class RoomFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? Code { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? FloorId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? RoomCategoryId { get; set; }

        [FilterField(FilterOperationEnum.Equal,
            TargetField: $"{nameof(Room.Floor)}.{nameof(Floor.Building)}.{nameof(Building.Id)}")]
        public int? BuildingId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? SpecialtyId { get; set; }
    }
}
