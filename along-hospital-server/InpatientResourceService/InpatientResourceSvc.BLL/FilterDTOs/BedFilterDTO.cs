using SharedLibrary.Commons.Filters;

namespace InpatientResourceSvc.BLL.FilterDTOs
{
    public class BedFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? RoomId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? BedCategoryId { get; set; }
    }
}