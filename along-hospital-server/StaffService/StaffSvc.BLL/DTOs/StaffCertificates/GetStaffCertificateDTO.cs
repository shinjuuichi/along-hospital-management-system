using SharedLibrary.Base.Mappers;
using StaffSvc.BLL.DTOs.StaffCertificateTypes;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffCertificates
{
    public class GetStaffCertificateDTO : MapFrom<StaffCertificate>
    {
        public int Id { get; set; }

        public string? CertificateNo { get; set; }

        public DateOnly IssuedDate { get; set; }

        public DateOnly ExpiredDate { get; set; }

        public string? IssuedBy { get; set; }

        public string? Status { get; set; }

        public string? Reason { get; set; }

        public int StaffCertificateTypeId { get; set; }

        public GetStaffCertificateTypeDTO? StaffCertificateType { get; set; }

        public int StaffId { get; set; }

        public string? StaffName { get; set; }
    }
}