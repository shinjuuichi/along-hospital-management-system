using SharedLibrary.Commons.Results;
using VoucherSvc.BLL.DTOs.PatientVoucherDTOs;
using VoucherSvc.BLL.DTOs.VoucherDTOs;
using VoucherSvc.BLL.FilterDTOs;

namespace VoucherSvc.BLL.Interfaces
{
    public interface IPatientVoucherService
    {
        Task CollectVoucherForNewPatientAsync(int patientId);

        Task<PaginationResult<GetVoucherDTO>> GetCollectibleVouchersAsync(VoucherFilterDTO filter);

        Task SelfCollectVoucherAsync(CollectVoucherDTO request);

        Task<PaginationResult<GetMyPatientVoucherDTO>> GetMyVouchersAsync(PatientVoucherFilterDTO filter);

        Task<List<GetMyPatientVoucherDTO>> GetAllMyVouchersAsync();
    }
}
