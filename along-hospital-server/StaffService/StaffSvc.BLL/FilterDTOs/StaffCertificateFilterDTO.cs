using SharedLibrary.Commons.Filters;

namespace StaffSvc.BLL.FilterDTOs
{
    public class StaffCertificateFilterDTO : FilterDTO
    {
        [FilterField(FilterOperationEnum.Contains)]
        public string? CertificateNo { get; set; }

        [FilterField(FilterOperationEnum.Contains)]
        public string? IssuedBy { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? StaffCertificateTypeId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public int? StaffId { get; set; }

        [FilterField(FilterOperationEnum.Equal)]
        public string? Status { get; set; }
    }
}