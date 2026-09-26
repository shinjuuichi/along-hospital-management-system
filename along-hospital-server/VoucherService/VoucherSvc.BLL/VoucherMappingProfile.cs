using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;
using VoucherSvc.BLL.DTOs.DiscountDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.ApplyVoucherDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewListMedicineDiscountDTOs;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewVoucherDTOs;

namespace VoucherSvc.BLL
{
    public class VoucherMappingProfile : BaseMappingProfile
    {
        public VoucherMappingProfile()
            : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<MedicineDetailEventItem, MedicineItemDTO>();
            CreateMap<ApplyVoucherEvent, ApplyVoucherRequestDTO>();
            CreateMap<ApplyVoucherResponseDTO, ApplyVoucherContract>();
            CreateMap<MedicineDiscountDetailDTO, MedicineDiscountDetailContract>();

            CreateMap<PreviewVoucherEvent, PreviewVoucherRequestDTO>();
            CreateMap<PreviewVoucherResponseDTO, PreviewVoucherContract>();

            CreateMap<PreviewMedicineDetailEvent, PreviewMedicineItemDTO>();
            CreateMap<PreviewListMedicineDiscountEvent, PreviewListMedicineDiscountRequestDTO>();
            CreateMap<PreviewListMedicineDiscountResponseDTO, PreviewListMedicineDiscountContract>();
            CreateMap<MedicineDiscountDetailDTO, MedicineDiscountDetailContract>();
        }
    }
}
