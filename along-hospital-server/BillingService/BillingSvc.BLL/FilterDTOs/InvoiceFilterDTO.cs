using BillingSvc.DAL.Models;
using SharedLibrary.Commons.Filters;

namespace BillingSvc.BLL.FilterDTOs
{
    public class InvoiceFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains, nameof(Invoice.InvoiceNumber))]
        public string? InvoiceNumber { get; set; }

        [FilterField(FilterOperationEnum.Equal, nameof(Invoice.InvoiceStatus))]
        public string? InvoiceStatus { get; set; }

        [FilterField(FilterOperationEnum.GreaterThanOrEqual, nameof(Invoice.PaymentDate))]
        public DateTime? PaymentDateFrom { get; set; }

        [FilterField(FilterOperationEnum.LessThanOrEqual, nameof(Invoice.PaymentDate))]
        public DateTime? PaymentDateTo { get; set; }
    }
}
