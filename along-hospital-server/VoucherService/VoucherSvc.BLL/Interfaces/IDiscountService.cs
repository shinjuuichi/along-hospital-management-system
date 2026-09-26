using VoucherSvc.BLL.DTOs.DiscountDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.ApplyVoucherDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewListMedicineDiscountDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewVoucherDTOs;

namespace VoucherSvc.BLL.Interfaces
{
    public interface IDiscountService
    {
        Task<ApplyVoucherResponseDTO> ApplyVoucherAsync(ApplyVoucherRequestDTO request);
        Task<MedicineDiscountDetailDTO> PreviewMedicineDiscountAsync(PreviewMedicineItemDTO request);
        Task<PreviewListMedicineDiscountResponseDTO> PreviewListMedicineDiscountAsync(PreviewListMedicineDiscountRequestDTO request);
        Task<PreviewVoucherResponseDTO> PreviewVoucherAsync(PreviewVoucherRequestDTO request);
    }
}
