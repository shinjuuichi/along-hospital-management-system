using SharedLibrary.Base.Mappers;
using StaffSvc.DAL.Models;

namespace StaffSvc.BLL.DTOs.StaffCertificates
{
    public class UpsertStaffCertificateDTO : MapTo<StaffCertificate>
    {
        public string? CertificateNo { get; set; }

        public DateOnly IssuedDate { get; set; }

        public DateOnly ExpiredDate { get; set; }

        public string? IssuedBy { get; set; }

        public int StaffCertificateTypeId { get; set; }

        public int StaffId { get; set; }
    }
}