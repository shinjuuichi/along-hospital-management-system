using SharedLibrary.Base.Services;
using StaffSvc.BLL.DTOs.StaffCertificates;

namespace StaffSvc.BLL.Interfaces
{
    public interface IStaffCertificateService : IBaseCrudService<UpsertStaffCertificateDTO, UpsertStaffCertificateDTO, GetStaffCertificateDTO>
    {
        Task ExpireCertificatesAsync();
        Task SendExpirationReminderEmailsAsync();
        Task SuspendCertificatesAsync(int id, string reason);
        Task ActiveCertificatesAsync(int id, DateOnly expiredDate);
        Task ApproveCertificatesAsync(int id);
    }
}