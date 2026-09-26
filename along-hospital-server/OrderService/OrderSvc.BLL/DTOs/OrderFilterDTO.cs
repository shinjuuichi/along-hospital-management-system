using SharedLibrary.Commons.Filters;

namespace OrderSvc.BLL.DTOs
{
    public class OrderFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Equal)]
        public string? OrderStatus { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public DateTime? OrderDate { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public DateTime? DeliveryDate { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? IsPickupAtStore { get; set; }
    }
}
