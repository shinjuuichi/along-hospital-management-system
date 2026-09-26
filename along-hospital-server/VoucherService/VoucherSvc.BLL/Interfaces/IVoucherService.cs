using SharedLibrary.Commons.Results;
using VoucherSvc.BLL.DTOs.VoucherDTOs;
using VoucherSvc.BLL.FilterDTOs;

namespace VoucherSvc.BLL.Interfaces
{
    public interface IVoucherService
    {
        Task<GetVoucherDTO> CreateAsync(CreateVoucherDTO dto);

        Task<GetVoucherDTO> UpdateAsync(string id, UpdateVoucherDTO dto);

        Task DeleteAsync(string id);

        Task<GetVoucherDTO> GetByIdAsync(string id);

        Task<PaginationResult<GetVoucherDTO>> GetAllAsync(VoucherFilterDTO filter);

        Task ExpireVouchersAsync();
    }
}
